using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Capybara_friend : MonoBehaviour
{
    [SerializeField]
    [Header("친구 카피바라 순서")]
    private int number;

    [SerializeField]
    [Header("대장 카피바라")]
    private GameObject captain;

    [SerializeField]
    [Header("대장 카피바라와의 거리")]
    private float distanceToPlayer;

    [SerializeField]
    [Header("대장 카피바라 Vector")]
    private float vectorToPlayer;

    [SerializeField]
    [Header("점프 파워")]
    private float jumpPower = 30f; // 점프 파워

    [SerializeField]
    [Header("따라가는 속도")]
    private float followSpeed = 0.5f; // 따라가는 거리

    [SerializeField]
    [Header("위치 오프셋")]
    private float followDistanceOffset = 3f; // 따라가는 거리

    [SerializeField]
    [Header("따라가지 않는 거리")]
    private float unfollowDistance = 3f; // 따라가는 범위 -> followDistance + unfollowDistance 이상으로 벌어지면 Miss 상태!

    [SerializeField]
    [Header("올라가 있는지")]
    private bool isStack;

    [SerializeField]
    [Header("대장 잃어버렸는지")]
    private bool isMissing;

    // 컴포넌트
    private Rigidbody2D rigidbody;
    
    private void OnEnable() // 오브젝트 생성시 호출
    {
        captain = GameObject.FindWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();

        isStack = false; // 생성될 때 카피바라는 올라가있지 않음
        isMissing = false;
    }

    public void Initialize(int number, float followDistance) // 초기화 함수
    {
        this.number = number;
        this.followDistanceOffset = followDistance;
    }
    public void RePosition() // 위치 재설정
    {
        this.followDistanceOffset -= 3;
    }

    private void Update()
    {
        FindCaptaion();

        if(isStack && (this.transform.localPosition.x < 0)) // 쌓아져있다가 튕겨져서 머리위에서 떨어짐.
        {
            FriendManager.friendManager.MissingFromHead(this);
            FriendManager.friendManager.EnqueueToTail(this);
        }
    }

    private void FixedUpdate()
    {
        if (!isMissing && !isStack)
            Follow();
    }

    // 스택 상태였다가.
    // 벽맞아서 머리에서 떨어졌으면
    // head에서 Dequeue하고
    // tail에 Enqueue하면 됨

    void Follow()
    {
        // 대열 or 대장을 향한 벡터 계산 : vectorToPlayer
        vectorToPlayer = captain.transform.position.x - transform.position.x; // 대장 카피바라를 향한 방향

        if (Mathf.Abs(vectorToPlayer) < followDistanceOffset) // vectorToPlayer 보다 작으면 더이상 쫓아가지 않음
        {
            /*if (vectorToPlayer == Vector2.left)
                transform.position += Vector3.right * followSpeed * 0.1f; // 왼쪽으로 쫓아가기
            else if (vectorToPlayer == Vector2.right)
                transform.position += Vector3.left * followSpeed * 0.1f; // 오른쪽으로 쫓아가기*/
        }
        else // vectorToPlayer 보다 크거나 같으면 쫓아서 이동
        {
            if (vectorToPlayer <= 0)
                transform.position += Vector3.left * followSpeed; // 왼쪽으로 쫓아가기
            else if (vectorToPlayer > 0)
                transform.position += Vector3.right * followSpeed; // 오른쪽으로 쫓아가기
        }
    }

    void FindCaptaion()
    {
        // 대장과의 거리 계산 : distanceToPlayer
        distanceToPlayer = Vector2.Distance(captain.transform.position, this.transform.position);

        if (isMissing) // 대장을 잃어버린 상태 (대장을 잃어버려서 찾는 상태)
        {
            if (distanceToPlayer < followDistanceOffset)
            {
                Discovering();
            } 
        }
        else // 대장에 속해있는 상태 (대장을 잃어버릴지 찾는 상태)
        {
            // followDistnaceOffset 보다 훨씬 멀어지면 (followDistnaceOffset*2 정도?) 더 이상 쫓아가지 않고 그 자리에서 찾기만 계속함(Missing 상태)
            if (distanceToPlayer > followDistanceOffset * 3)
            {
                Missing();
            }
        }
    }

    void Missing() // 대장을 잃어버림. (뺏김, 걸려서 못 쫓아감 등)
    {
        isMissing = true;
        this.transform.parent = FriendManager.friendManager.gameObject.transform; // 부모를 FriendManager로
        FriendManager.friendManager.DequeueFromTail(this); // 큐에서 자기 자신 빼기
        /*Debug.Log(this.name + "잃어버림");
        Debug.Log("거리 : " + distanceToPlayer);*/
    }

    public bool GetMissing()
    {
        return isMissing;
    }
    void Discovering() // 대장을 찾음!
    {
        isMissing = false;
        this.transform.parent = captain.transform; // 부모를 Captain으로
        FriendManager.friendManager.EnqueueToTail(this); // 큐에서 자기 자신 넣기
        /*Debug.Log(this.name + "대장 찾음!");
        Debug.Log("거리 : " + distanceToPlayer);*/
    }

    public void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public void PushOnHead() // 머리에 얹혀질 때
    {
        isStack = true;
        rigidbody.excludeLayers = LayerMask.GetMask("Nothing");
        
    }
    public void PopFromHead() // 머리에서 내릴 때
    {
        isStack = false;
        rigidbody.excludeLayers = LayerMask.GetMask("Capybara");
    }
}
