using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class FriendManager : MonoBehaviour
{
    public static FriendManager friendManager;

    [SerializeField]
    [Header("카피바라 친구들 생성 수")]
    private int friend_Count;

    [SerializeField]
    [Header("카피바라 친구들 점프 딜레이")]
    private float jumpDelay;

    [SerializeField]
    [Header("카피바라 친구들 오프셋")]
    private float friendsOffset = 3;

    [SerializeField]
    [Header("대장 카피바라")]
    GameObject captain;

    [SerializeField]
    [Header("회전 가능")]
    private bool canRotate;

    [SerializeField]
    [Header("점프 가능")]
    private bool canJump;

    [SerializeField]
    [Header("대장 카피바라 머리 위치")]
    private GameObject headofCaptain;

    [SerializeField]
    [Header("머리 위 상수")]
    private float headConstant = 2;

    [SerializeField]
    [Header("카피바라 친구들 꼬리 Queue")]
    Queue<Capybara_friend> friends_on_tail = new Queue<Capybara_friend>();

    [SerializeField]
    [Header("카피바라 친구들 머리 Queue")]
    Queue<Capybara_friend> friends_on_head = new Queue<Capybara_friend>();

    [Header("카피바라 친구 프리팹")]
    public GameObject friend_prefab;

    public bool CanRotate()
    {
        return canRotate;
    }
    public bool CanJump()
    {
        return canJump;
    }

    private void Awake()
    {
        friendManager = this;

        canRotate = true; // 친구가 하나 이상 있을 경우 Rotate 불가!
        canJump = true; // 머리위에 친구가 없을 경우 Jump 가능
    }

    private void Start()
    {
        // 캡틴이 바라보는 방향 가져오기
        for (int i = 0; i < friend_Count; i++)
        {
            GameObject friend = Instantiate(friend_prefab, this.transform);

            // 생성된 카피바라 Enqueue
            friends_on_tail.Enqueue(friend.GetComponent<Capybara_friend>());

            // 부모 설정 및 위치 초기화
            friend.transform.parent = captain.transform;
            friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);

            // 생성된 카피바라 친구들 초기화(순서, 거리 등)
            friend.GetComponent<Capybara_friend>().Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset); // (순서, 거리)
            friend.gameObject.name = "카피바라 " + (friends_on_tail.Count) + "번째";

            StateCheck();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // 쌓기
        {
            EnqueueToHead();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) // 내리기
        {
            DequeueFromHead();
        }
    }
    private void EnqueueToHead() // 카피바라 친구들을 대장 머리 위로 쌓기
    {
        if (friends_on_tail.Count > 0 && !captain.GetComponent<Capybara_Move>().GetJuming()) // 올릴 카피바라가 존재하면
        {
            Capybara_friend friend = friends_on_tail.Dequeue(); // 꼬리 Queue에서 Dequeue
            friends_on_head.Enqueue(friend); // 머리 Queue에 Enqueue

            friend.transform.parent = headofCaptain.transform; // 대장 카피바라의 머리를 부모로 취급
            friend.transform.position = headofCaptain.transform.position + new Vector3(0, (friends_on_head.Count) * headConstant, 0); // 실제 위치 이동

            friend.PushOnHead();

            foreach (Capybara_friend otherFriends in friends_on_tail) // 꼬리에 남은 다른 친구들 앞으로 당기기
            {
                otherFriends.RePosition(); // 위치 재설정
            }
        }

        StateCheck();
    }

    private void DequeueFromHead() // 카피바라 친구들을 대장 머리에서 내리기
    {
        if (friends_on_head.Count > 0 && !captain.GetComponent<Capybara_Move>().GetJuming()) // 내릴 카피바라가 존재하면
        {
            Capybara_friend friend = friends_on_head.Dequeue();
            friends_on_tail.Enqueue(friend);
            friend.PopFromHead();

            friend.transform.parent = captain.transform; // 대장 카피바라를 부모로 취급

            if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
                // 왼쪽을 보고 있을 때
                friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);
            else
                // 오른쪽을 보고 있을 때
                friend.transform.position = captain.transform.position + new Vector3(-(friends_on_tail.Count) * friendsOffset, 0, 0);

            

            // 위치 재설정
            friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);
        }

        StateCheck();
    }

    public void MissingFromHead(Capybara_friend capybara)
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_head.Count;
        // 해당 카피바라 꼬리 Queue에서 제거
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_head.Dequeue();
            if (capybara != f)
                tmp.Enqueue(f);
        }

        friends_on_head.Clear();

        int cnt2 = tmp.Count;
        // 꼬리 큐 재 설정 및 친구들 순서 위치 재정의;
        for (int i = 0; i < cnt2; i++)
        {
            Capybara_friend f = tmp.Dequeue();
            friends_on_head.Enqueue(f);
        }

        tmp.Clear();
        capybara.PopFromHead();
        StateCheck();
    }

    public void AllFriendJump()
    {
        StartCoroutine(AllFriendJumpCoroutine());
    }
    private IEnumerator AllFriendJumpCoroutine()
    {
        foreach (Capybara_friend friend in friends_on_tail)
        {
            yield return new WaitForSeconds(jumpDelay);
            if(!friend.GetMissing())
                friend.Jump();
        }
    }

    public void DequeueFromTail(Capybara_friend capybara) // 꼬리에서 벗어난 카피바라 처리
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_tail.Count;
        // 해당 카피바라 꼬리 Queue에서 제거
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_tail.Dequeue();
            if (capybara != f)
                tmp.Enqueue(f);
        }
       
        friends_on_tail.Clear();

        int cnt2 = tmp.Count;
        // 꼬리 큐 재 설정 및 친구들 순서 위치 재정의;
        for (int i = 0; i < cnt2; i++)
        {
            Capybara_friend f = tmp.Dequeue();
            friends_on_tail.Enqueue(f);
            f.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);
        }

        tmp.Clear();

        StateCheck();
    }

    public void EnqueueToTail(Capybara_friend capybara_friend)
    {
        friends_on_tail.Enqueue(capybara_friend);
        capybara_friend.PopFromHead();

        if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
            // 왼쪽을 보고 있을 때
            capybara_friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);
        else
            // 오른쪽을 보고 있을 때
            capybara_friend.transform.position = captain.transform.position + new Vector3(-(friends_on_tail.Count) * friendsOffset, 0, 0);

        capybara_friend.transform.localScale = new Vector3(capybara_friend.transform.localScale.x, Math.Abs(capybara_friend.transform.localScale.y), capybara_friend.transform.localScale.z); // 스케일 초기화
        capybara_friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);

        StateCheck();
    }

    public void CaptainTaken() // 대장 카피바라가 잡힘
    {
        foreach (Capybara_friend friend in friends_on_head) // 머리에 존재하던 카피바라들 내리기
        {
            DequeueFromHead();
        }

        foreach (Capybara_friend friend in friends_on_tail) // 꼬리에 존재하던 카피바라들 내리기
        {
            DequeueFromTail(friend);
        }
    }

    void StateCheck()
    {
        if (friends_on_tail.Count > 0) canRotate = false; // 회전 제어
        else canRotate = true;

        if (friends_on_head.Count > 0) canJump = false; // 점프 제어
        else canJump = true;
    }
}
