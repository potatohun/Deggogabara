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

    [SerializeField]
    [Header("합류 가능")]
    public bool canJoin;

    [SerializeField]
    [Header("상태 표시창")]
    private GameObject stateNotification;

    [SerializeField]
    [Header("알림 아이콘")]
    private GameObject[] notificationIcon;

    private float distanceToTail;

    // 컴포넌트
    private Rigidbody2D rigidbody;
    private Animator animator;
    private GroundCheck groundCheck;
    
    private void OnEnable() // 오브젝트 생성시 호출
    {
        captain = GameObject.FindWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();

        isStack = false; // 생성될 때 카피바라는 올라가있지 않음
        isMissing = true;
        canJoin = false;

        PopFromHead();

        stateNotification.SetActive(false);
    }

    public void Initialize(int number, float followDistance) // 초기화 함수
    {
        isMissing = false;
        this.number = number;
        this.followDistanceOffset = followDistance;
    }
    public void RePosition() // 위치 재설정
    {
        this.followDistanceOffset -= FriendManager.friendManager.FriendOffset(); ;
    }

    private void Update()
    {
        FindCaptaion();

        if (groundCheck.IsGround())// 땅바닥 감지 (점프 중 확인)
        {
            animator.SetBool("isJump", false);
        }
        else
        {
            animator.SetBool("isJump", true);
        }

        if (!isMissing)
        {
            if (isStack) // 머리 위 상태일 때 
            {
                if ((this.transform.localPosition.y - captain.transform.position.y) < 0) // 쌓아져있다가 튕겨져서 머리위에서 떨어짐.)
                {
                    Debug.Log("튕겨져서 떨어짐");
                    Missing();
                }
            }
            else // 꼬리 상태 일때
            {
                if (!FriendManager.friendManager.GetAllJumping() && distanceToPlayer > followDistanceOffset * 3f)
                {
                    Debug.Log("점프에서 떨어짐");
                    Debug.Log(distanceToPlayer);
                    Missing();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if ((!isMissing) && (!isStack)) // 잃어버린 상태가 아니고, 쌓여져있는 상태가 아닐때 쫓아가기
        {
            Follow();
        }
        else if (isMissing) // 잃어버린 상태 일 때
        {
            distanceToPlayer = Vector2.Distance(captain.transform.position, this.transform.position);

            // followDistnaceOffset 보다 훨씬 멀어지면 (followDistnaceOffset*2 정도?) 더 이상 쫓아가지 않고 그 자리에서 찾기만 계속함(Missing 상태)
            if (distanceToPlayer < followDistanceOffset)
            {
                canJoin = true;
                ChangeIcon(false); // 찾으러가겠다는 아이콘으로 설정
                EnableNotification();
            }
            else
            {
                canJoin = false;
                ChangeIcon(true); // 잃어버린 아이콘으로 설정
                EnableNotification();
            }
        }
    }

    void Follow()
    {
        // 대열 or 대장을 향한 벡터 계산 : vectorToPlayer
        distanceToTail = captain.GetComponent<Capybara_Move>().tailPosition.position.x - transform.position.x; // 대장 카피바라를 향한 방향
        if (Mathf.Abs(distanceToTail) > followDistanceOffset) // vectorToPlayer 보다 작으면 더이상 쫓아가지 않음
        {
            animator.SetBool("isMove", true);
            if (distanceToTail <= 0)
                transform.position += Vector3.left * followSpeed; // 왼쪽으로 쫓아가기
            else if (distanceToTail > 0)
                transform.position += Vector3.right * followSpeed; // 오른쪽으로 쫓아가기
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    void FindCaptaion()
    {
        if (!isMissing)
        {
            // 대장과의 거리 계산 : distanceToPlayer
            distanceToTail = Vector2.Distance(captain.GetComponent<Capybara_Move>().tailPosition.position, this.transform.position);

            // followDistnaceOffset 보다 훨씬 멀어지면 (followDistnaceOffset*2 정도?) 더 이상 쫓아가지 않고 그 자리에서 찾기만 계속함(Missing 상태)
            if (distanceToTail > (followDistanceOffset * 5))
            {
                Debug.Log("거리가 멀어져서 떨어짐");
                Missing(); // 거리가 멀어지면 대열에서 잃어버려짐
            }
        }
    }

    public void Missing() // 대장을 잃어버림. (뺏김, 걸려서 못 쫓아감 등)
    {
        isMissing = true;
        canJoin = true;
        isStack = false;

        animator.SetBool("isMove", false);
        animator.SetBool("isJump", false);
        PopFromHead();

        this.transform.parent = FriendManager.friendManager.gameObject.transform; // 부모를 FriendManager로
        this.number = 0;
        this.followDistanceOffset = FriendManager.friendManager.FriendOffset();
        FriendManager.friendManager.MissingFromHead(this); // 큐에서 자기 자신 빼기
        FriendManager.friendManager.DequeueFromTail(this); // 큐에서 자기 자신 빼기
    }

    public bool GetMissing()
    {
        return isMissing;
    }

    public void JoinToGroup() // 대장을 찾음!
    {
        DisableNotification();
        isMissing = false;
        isStack = false;
        canJoin = false;
        //this.transform.parent = captain.transform; // 부모를 Captain으로
        FriendManager.friendManager.EnqueueToTail(this); // 큐에서 자기 자신 넣기
    }

    public void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        animator.SetBool("isJump", true);
    }
    bool DetectGround() // 땅바닥 감지
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.down, Vector2.down, 1f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Player") || hit.collider.CompareTag("Friends"))
            {
                return true; // Ground 감지 O
            }
        }
        return false; // Ground 감지 X
    }
    public void PushOnHead() // 머리에 얹혀질 때
    {
        isStack = true;
        rigidbody.excludeLayers = LayerMask.GetMask("Nothing");
        //rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public void PopFromHead() // 머리에서 내릴 때
    {
        isStack = false;
        rigidbody.excludeLayers = LayerMask.GetMask("Capybara");
        //rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void EnableNotification()
    {
        stateNotification.SetActive(true);
    }

    public void DisableNotification()
    {
        stateNotification.SetActive(false);
    }

    void ChangeIcon(bool isMiss)
    {
        if (isMiss)
        {
            notificationIcon[0].SetActive(false);
            notificationIcon[1].SetActive(true);
        }
        else
        {
            notificationIcon[1].SetActive(false);
            notificationIcon[0].SetActive(true);
        }
    }

    public float DistanceToCaptain()
    {
        return distanceToPlayer;
    }
}
