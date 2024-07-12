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
    [Header("ī�ǹٶ� ģ���� ���� ��")]
    private int friend_Count;

    [SerializeField]
    [Header("ī�ǹٶ� ģ���� ���� ������")]
    private float jumpDelay;

    [SerializeField]
    [Header("ī�ǹٶ� ģ���� ������")]
    private float friendsOffset = 3;

    [SerializeField]
    [Header("���� ī�ǹٶ�")]
    GameObject captain;

    [SerializeField]
    [Header("ȸ�� ����")]
    private bool canRotate;

    [SerializeField]
    [Header("���� ����")]
    private bool canJump;

    [SerializeField]
    [Header("���� ī�ǹٶ� �Ӹ� ��ġ")]
    private GameObject headofCaptain;

    [SerializeField]
    [Header("�Ӹ� �� ���")]
    private float headConstant = 2;

    [SerializeField]
    [Header("ī�ǹٶ� ģ���� ���� Queue")]
    Queue<Capybara_friend> friends_on_tail = new Queue<Capybara_friend>();

    [SerializeField]
    [Header("ī�ǹٶ� ģ���� �Ӹ� Queue")]
    Queue<Capybara_friend> friends_on_head = new Queue<Capybara_friend>();

    [Header("ī�ǹٶ� ģ�� ������")]
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

        canRotate = true; // ģ���� �ϳ� �̻� ���� ��� Rotate �Ұ�!
        canJump = true; // �Ӹ����� ģ���� ���� ��� Jump ����
    }

    private void Start()
    {
        // ĸƾ�� �ٶ󺸴� ���� ��������
        for (int i = 0; i < friend_Count; i++)
        {
            GameObject friend = Instantiate(friend_prefab, this.transform);

            // ������ ī�ǹٶ� Enqueue
            friends_on_tail.Enqueue(friend.GetComponent<Capybara_friend>());

            // �θ� ���� �� ��ġ �ʱ�ȭ
            friend.transform.parent = captain.transform;
            friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);

            // ������ ī�ǹٶ� ģ���� �ʱ�ȭ(����, �Ÿ� ��)
            friend.GetComponent<Capybara_friend>().Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset); // (����, �Ÿ�)
            friend.gameObject.name = "ī�ǹٶ� " + (friends_on_tail.Count) + "��°";

            StateCheck();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // �ױ�
        {
            EnqueueToHead();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) // ������
        {
            DequeueFromHead();
        }
    }
    private void EnqueueToHead() // ī�ǹٶ� ģ������ ���� �Ӹ� ���� �ױ�
    {
        if (friends_on_tail.Count > 0 && !captain.GetComponent<Capybara_Move>().GetJuming()) // �ø� ī�ǹٶ� �����ϸ�
        {
            Capybara_friend friend = friends_on_tail.Dequeue(); // ���� Queue���� Dequeue
            friends_on_head.Enqueue(friend); // �Ӹ� Queue�� Enqueue

            friend.transform.parent = headofCaptain.transform; // ���� ī�ǹٶ��� �Ӹ��� �θ�� ���
            friend.transform.position = headofCaptain.transform.position + new Vector3(0, (friends_on_head.Count) * headConstant, 0); // ���� ��ġ �̵�

            friend.PushOnHead();

            foreach (Capybara_friend otherFriends in friends_on_tail) // ������ ���� �ٸ� ģ���� ������ ����
            {
                otherFriends.RePosition(); // ��ġ �缳��
            }
        }

        StateCheck();
    }

    private void DequeueFromHead() // ī�ǹٶ� ģ������ ���� �Ӹ����� ������
    {
        if (friends_on_head.Count > 0 && !captain.GetComponent<Capybara_Move>().GetJuming()) // ���� ī�ǹٶ� �����ϸ�
        {
            Capybara_friend friend = friends_on_head.Dequeue();
            friends_on_tail.Enqueue(friend);
            friend.PopFromHead();

            friend.transform.parent = captain.transform; // ���� ī�ǹٶ� �θ�� ���

            if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
                // ������ ���� ���� ��
                friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);
            else
                // �������� ���� ���� ��
                friend.transform.position = captain.transform.position + new Vector3(-(friends_on_tail.Count) * friendsOffset, 0, 0);

            

            // ��ġ �缳��
            friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);
        }

        StateCheck();
    }

    public void MissingFromHead(Capybara_friend capybara)
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_head.Count;
        // �ش� ī�ǹٶ� ���� Queue���� ����
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_head.Dequeue();
            if (capybara != f)
                tmp.Enqueue(f);
        }

        friends_on_head.Clear();

        int cnt2 = tmp.Count;
        // ���� ť �� ���� �� ģ���� ���� ��ġ ������;
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

    public void DequeueFromTail(Capybara_friend capybara) // �������� ��� ī�ǹٶ� ó��
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_tail.Count;
        // �ش� ī�ǹٶ� ���� Queue���� ����
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_tail.Dequeue();
            if (capybara != f)
                tmp.Enqueue(f);
        }
       
        friends_on_tail.Clear();

        int cnt2 = tmp.Count;
        // ���� ť �� ���� �� ģ���� ���� ��ġ ������;
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
            // ������ ���� ���� ��
            capybara_friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);
        else
            // �������� ���� ���� ��
            capybara_friend.transform.position = captain.transform.position + new Vector3(-(friends_on_tail.Count) * friendsOffset, 0, 0);

        capybara_friend.transform.localScale = new Vector3(capybara_friend.transform.localScale.x, Math.Abs(capybara_friend.transform.localScale.y), capybara_friend.transform.localScale.z); // ������ �ʱ�ȭ
        capybara_friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);

        StateCheck();
    }

    public void CaptainTaken() // ���� ī�ǹٶ� ����
    {
        foreach (Capybara_friend friend in friends_on_head) // �Ӹ��� �����ϴ� ī�ǹٶ�� ������
        {
            DequeueFromHead();
        }

        foreach (Capybara_friend friend in friends_on_tail) // ������ �����ϴ� ī�ǹٶ�� ������
        {
            DequeueFromTail(friend);
        }
    }

    void StateCheck()
    {
        if (friends_on_tail.Count > 0) canRotate = false; // ȸ�� ����
        else canRotate = true;

        if (friends_on_head.Count > 0) canJump = false; // ���� ����
        else canJump = true;
    }
}
