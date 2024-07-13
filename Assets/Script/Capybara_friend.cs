using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Capybara_friend : MonoBehaviour
{
    [SerializeField]
    [Header("ģ�� ī�ǹٶ� ����")]
    private int number;

    [SerializeField]
    [Header("���� ī�ǹٶ�")]
    private GameObject captain;

    [SerializeField]
    [Header("���� ī�ǹٶ���� �Ÿ�")]
    private float distanceToPlayer;

    [SerializeField]
    [Header("���� ī�ǹٶ� Vector")]
    private float vectorToPlayer;

    [SerializeField]
    [Header("���� �Ŀ�")]
    private float jumpPower = 30f; // ���� �Ŀ�

    [SerializeField]
    [Header("���󰡴� �ӵ�")]
    private float followSpeed = 0.5f; // ���󰡴� �Ÿ�

    [SerializeField]
    [Header("��ġ ������")]
    private float followDistanceOffset = 3f; // ���󰡴� �Ÿ�

    [SerializeField]
    [Header("������ �ʴ� �Ÿ�")]
    private float unfollowDistance = 3f; // ���󰡴� ���� -> followDistance + unfollowDistance �̻����� �������� Miss ����!

    [SerializeField]
    [Header("�ö� �ִ���")]
    private bool isStack;

    [SerializeField]
    [Header("���� �Ҿ���ȴ���")]
    private bool isMissing;

    [SerializeField]
    [Header("�շ� ����")]
    public bool canJoin;

    [SerializeField]
    [Header("���� ǥ��â")]
    private GameObject stateNotification;

    [SerializeField]
    [Header("�˸� ������")]
    private GameObject[] notificationIcon;

    private float distanceToTail;

    // ������Ʈ
    private Rigidbody2D rigidbody;
    private Animator animator;
    private GroundCheck groundCheck;
    
    private void OnEnable() // ������Ʈ ������ ȣ��
    {
        captain = GameObject.FindWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();

        isStack = false; // ������ �� ī�ǹٶ�� �ö����� ����
        isMissing = true;
        canJoin = false;

        PopFromHead();

        stateNotification.SetActive(false);
    }

    public void Initialize(int number, float followDistance) // �ʱ�ȭ �Լ�
    {
        isMissing = false;
        this.number = number;
        this.followDistanceOffset = followDistance;
    }
    public void RePosition() // ��ġ �缳��
    {
        this.followDistanceOffset -= FriendManager.friendManager.FriendOffset(); ;
    }

    private void Update()
    {
        FindCaptaion();

        if (groundCheck.IsGround())// ���ٴ� ���� (���� �� Ȯ��)
        {
            animator.SetBool("isJump", false);
        }
        else
        {
            animator.SetBool("isJump", true);
        }

        if (!isMissing)
        {
            if (isStack) // �Ӹ� �� ������ �� 
            {
                if ((this.transform.localPosition.y - captain.transform.position.y) < 0) // �׾����ִٰ� ƨ������ �Ӹ������� ������.)
                {
                    Debug.Log("ƨ������ ������");
                    Missing();
                }
            }
            else // ���� ���� �϶�
            {
                if (!FriendManager.friendManager.GetAllJumping() && distanceToPlayer > followDistanceOffset * 3f)
                {
                    Debug.Log("�������� ������");
                    Debug.Log(distanceToPlayer);
                    Missing();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if ((!isMissing) && (!isStack)) // �Ҿ���� ���°� �ƴϰ�, �׿����ִ� ���°� �ƴҶ� �Ѿư���
        {
            Follow();
        }
        else if (isMissing) // �Ҿ���� ���� �� ��
        {
            distanceToPlayer = Vector2.Distance(captain.transform.position, this.transform.position);

            // followDistnaceOffset ���� �ξ� �־����� (followDistnaceOffset*2 ����?) �� �̻� �Ѿư��� �ʰ� �� �ڸ����� ã�⸸ �����(Missing ����)
            if (distanceToPlayer < followDistanceOffset)
            {
                canJoin = true;
                ChangeIcon(false); // ã�������ڴٴ� ���������� ����
                EnableNotification();
            }
            else
            {
                canJoin = false;
                ChangeIcon(true); // �Ҿ���� ���������� ����
                EnableNotification();
            }
        }
    }

    void Follow()
    {
        // �뿭 or ������ ���� ���� ��� : vectorToPlayer
        distanceToTail = captain.GetComponent<Capybara_Move>().tailPosition.position.x - transform.position.x; // ���� ī�ǹٶ� ���� ����
        if (Mathf.Abs(distanceToTail) > followDistanceOffset) // vectorToPlayer ���� ������ ���̻� �Ѿư��� ����
        {
            animator.SetBool("isMove", true);
            if (distanceToTail <= 0)
                transform.position += Vector3.left * followSpeed; // �������� �Ѿư���
            else if (distanceToTail > 0)
                transform.position += Vector3.right * followSpeed; // ���������� �Ѿư���
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
            // ������� �Ÿ� ��� : distanceToPlayer
            distanceToTail = Vector2.Distance(captain.GetComponent<Capybara_Move>().tailPosition.position, this.transform.position);

            // followDistnaceOffset ���� �ξ� �־����� (followDistnaceOffset*2 ����?) �� �̻� �Ѿư��� �ʰ� �� �ڸ����� ã�⸸ �����(Missing ����)
            if (distanceToTail > (followDistanceOffset * 5))
            {
                Debug.Log("�Ÿ��� �־����� ������");
                Missing(); // �Ÿ��� �־����� �뿭���� �Ҿ������
            }
        }
    }

    public void Missing() // ������ �Ҿ����. (����, �ɷ��� �� �Ѿư� ��)
    {
        isMissing = true;
        canJoin = true;
        isStack = false;

        animator.SetBool("isMove", false);
        animator.SetBool("isJump", false);
        PopFromHead();

        this.transform.parent = FriendManager.friendManager.gameObject.transform; // �θ� FriendManager��
        this.number = 0;
        this.followDistanceOffset = FriendManager.friendManager.FriendOffset();
        FriendManager.friendManager.MissingFromHead(this); // ť���� �ڱ� �ڽ� ����
        FriendManager.friendManager.DequeueFromTail(this); // ť���� �ڱ� �ڽ� ����
    }

    public bool GetMissing()
    {
        return isMissing;
    }

    public void JoinToGroup() // ������ ã��!
    {
        DisableNotification();
        isMissing = false;
        isStack = false;
        canJoin = false;
        //this.transform.parent = captain.transform; // �θ� Captain����
        FriendManager.friendManager.EnqueueToTail(this); // ť���� �ڱ� �ڽ� �ֱ�
    }

    public void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        animator.SetBool("isJump", true);
    }
    bool DetectGround() // ���ٴ� ����
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + Vector3.down, Vector2.down, 1f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Player") || hit.collider.CompareTag("Friends"))
            {
                return true; // Ground ���� O
            }
        }
        return false; // Ground ���� X
    }
    public void PushOnHead() // �Ӹ��� ������ ��
    {
        isStack = true;
        rigidbody.excludeLayers = LayerMask.GetMask("Nothing");
        //rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public void PopFromHead() // �Ӹ����� ���� ��
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
