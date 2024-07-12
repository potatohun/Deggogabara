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

    // ������Ʈ
    private Rigidbody2D rigidbody;
    
    private void OnEnable() // ������Ʈ ������ ȣ��
    {
        captain = GameObject.FindWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();

        isStack = false; // ������ �� ī�ǹٶ�� �ö����� ����
        isMissing = false;
    }

    public void Initialize(int number, float followDistance) // �ʱ�ȭ �Լ�
    {
        this.number = number;
        this.followDistanceOffset = followDistance;
    }
    public void RePosition() // ��ġ �缳��
    {
        this.followDistanceOffset -= 3;
    }

    private void Update()
    {
        FindCaptaion();

        if(isStack && (this.transform.localPosition.x < 0)) // �׾����ִٰ� ƨ������ �Ӹ������� ������.
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

    // ���� ���¿��ٰ�.
    // ���¾Ƽ� �Ӹ����� ����������
    // head���� Dequeue�ϰ�
    // tail�� Enqueue�ϸ� ��

    void Follow()
    {
        // �뿭 or ������ ���� ���� ��� : vectorToPlayer
        vectorToPlayer = captain.transform.position.x - transform.position.x; // ���� ī�ǹٶ� ���� ����

        if (Mathf.Abs(vectorToPlayer) < followDistanceOffset) // vectorToPlayer ���� ������ ���̻� �Ѿư��� ����
        {
            /*if (vectorToPlayer == Vector2.left)
                transform.position += Vector3.right * followSpeed * 0.1f; // �������� �Ѿư���
            else if (vectorToPlayer == Vector2.right)
                transform.position += Vector3.left * followSpeed * 0.1f; // ���������� �Ѿư���*/
        }
        else // vectorToPlayer ���� ũ�ų� ������ �ѾƼ� �̵�
        {
            if (vectorToPlayer <= 0)
                transform.position += Vector3.left * followSpeed; // �������� �Ѿư���
            else if (vectorToPlayer > 0)
                transform.position += Vector3.right * followSpeed; // ���������� �Ѿư���
        }
    }

    void FindCaptaion()
    {
        // ������� �Ÿ� ��� : distanceToPlayer
        distanceToPlayer = Vector2.Distance(captain.transform.position, this.transform.position);

        if (isMissing) // ������ �Ҿ���� ���� (������ �Ҿ������ ã�� ����)
        {
            if (distanceToPlayer < followDistanceOffset)
            {
                Discovering();
            } 
        }
        else // ���忡 �����ִ� ���� (������ �Ҿ������ ã�� ����)
        {
            // followDistnaceOffset ���� �ξ� �־����� (followDistnaceOffset*2 ����?) �� �̻� �Ѿư��� �ʰ� �� �ڸ����� ã�⸸ �����(Missing ����)
            if (distanceToPlayer > followDistanceOffset * 3)
            {
                Missing();
            }
        }
    }

    void Missing() // ������ �Ҿ����. (����, �ɷ��� �� �Ѿư� ��)
    {
        isMissing = true;
        this.transform.parent = FriendManager.friendManager.gameObject.transform; // �θ� FriendManager��
        FriendManager.friendManager.DequeueFromTail(this); // ť���� �ڱ� �ڽ� ����
        /*Debug.Log(this.name + "�Ҿ����");
        Debug.Log("�Ÿ� : " + distanceToPlayer);*/
    }

    public bool GetMissing()
    {
        return isMissing;
    }
    void Discovering() // ������ ã��!
    {
        isMissing = false;
        this.transform.parent = captain.transform; // �θ� Captain����
        FriendManager.friendManager.EnqueueToTail(this); // ť���� �ڱ� �ڽ� �ֱ�
        /*Debug.Log(this.name + "���� ã��!");
        Debug.Log("�Ÿ� : " + distanceToPlayer);*/
    }

    public void Jump()
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public void PushOnHead() // �Ӹ��� ������ ��
    {
        isStack = true;
        rigidbody.excludeLayers = LayerMask.GetMask("Nothing");
        
    }
    public void PopFromHead() // �Ӹ����� ���� ��
    {
        isStack = false;
        rigidbody.excludeLayers = LayerMask.GetMask("Capybara");
    }
}
