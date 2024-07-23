using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FriendManager : MonoBehaviour
{
    public int tailCount;
    public int headCount;
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

    public bool IsTeamJumping;

    public AudioSource headSound;
    public AudioSource tailSound;
    public AudioSource errorSound;

    public Warning warning;
    public bool CanRotate()
    {
        return canRotate;
    }

    public void SetCanRotate(bool input)
    {
        canRotate = input;
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
        IsTeamJumping = false;
    }

    private void OnEnable()
    {
        // �� ������
        captain = GameObject.FindWithTag("Player");
        headofCaptain = GameObject.FindWithTag("Head");
    }


    private void Start()
    {
        // ĸƾ�� �ٶ󺸴� ���� ��������
        for (int i = 0; i < friend_Count; i++)
        {
            GameObject friend = Instantiate(friend_prefab, this.transform);

            // ������ ī�ǹٶ� Enqueue
            //friends_on_tail.Enqueue(friend.GetComponent<Capybara_friend>());
            EnqueueToTail(friend.GetComponent<Capybara_friend>());

            // �θ� ���� �� ��ġ �ʱ�ȭ
            //friend.transform.parent = captain.transform;
            //friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);

            // ������ ī�ǹٶ� ģ���� �ʱ�ȭ(����, �Ÿ� ��)
            friend.GetComponent<Capybara_friend>().Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset); // (����, �Ÿ�)
            friend.gameObject.name = "ī�ǹٶ� " + (friends_on_tail.Count) + "��°";

            StateCheck();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // �ױ�
        {
            if (!captain.GetComponent<Capybara_Move>().GetUpFloorStuck()) // �Ӹ��� ���� ������ �Ӹ� ���� ���װ� ��!
            {
                EnqueueToHead();
            }
            else
            {
                warning.PresentWarning("���� ���� �����־��!");
                errorSound.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // ������
        {
            if (!captain.GetComponent<Capybara_Move>().GetBackFloorStuck())
            {
                DequeueFromHead();
            }
            else
            {
                warning.PresentWarning("�ڿ� ���� �����־��!");
                errorSound.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) // ������
        {
            JoinFriend();
        }

        tailCount = friends_on_tail.Count;
        headCount = friends_on_head.Count;
    }
    private void EnqueueToHead() // ī�ǹٶ� ģ������ ���� �Ӹ� ���� �ױ�
    {
        if (friends_on_tail.Count > 0 && !captain.GetComponent<Capybara_Move>().GetJuming()) // �ø� ī�ǹٶ� �����ϸ�
        {
            if (Mathf.Abs(friends_on_tail.Peek().transform.position.y - captain.transform.position.y) > 3f)
            {
                warning.PresentWarning("���̰� �޶� ���� �� �����!");
                errorSound.Play();
                return;
            }

            headSound.Play();

            Capybara_friend friend = friends_on_tail.Dequeue(); // ���� Queue���� Dequeue
            

            friend.transform.parent = headofCaptain.transform; // ���� ī�ǹٶ��� �Ӹ��� �θ�� ���
            friend.transform.position = headofCaptain.transform.position + new Vector3(0, 1 + friends_on_head.Count * headConstant, 0); // ���� ��ġ �̵�
            friend.PushOnHead();

            friends_on_head.Enqueue(friend); // �Ӹ� Queue�� Enqueue
            foreach (Capybara_friend otherFriends in friends_on_tail) // ������ ���� �ٸ� ģ���� ������ ����
            {
                otherFriends.RePosition(); // ��ġ �缳��
            }
        }
        StateCheck();
    }

    public void DequeueFromHead() // ī�ǹٶ� ģ������ ���� �Ӹ����� ������
    {
        if (!captain.GetComponent<Capybara_Move>().GetJuming())
        {
            if (friends_on_head.Count > 0) // ���� ī�ǹٶ� �����ϸ�
            {
                Capybara_friend friend = friends_on_head.Dequeue();
                friends_on_tail.Enqueue(friend);
                friend.PopFromHead();

                friend.transform.parent = this.transform; // �Ŵ����� �θ�� ���

                if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
                    // ������ ���� ���� ��
                    friend.transform.position = captain.GetComponent<Capybara_Move>().tailPosition.position + new Vector3(friendsOffset, 0, 0);
                else
                    // �������� ���� ���� ��
                    friend.transform.position = captain.GetComponent<Capybara_Move>().tailPosition.position + new Vector3(-friendsOffset, 0, 0);

                // ��ġ �缳��
                friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);
            }
            else if (friends_on_tail.Count > 0) // ������ �ڸ� ī�ǹٶ� ������
            {
                //�����ڸ���
                CutTheTail();
            }
        }
        StateCheck();
    }

    private void CutTheTail()//���� �ڸ���
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_tail.Count;
        // �ش� ī�ǹٶ� ���� Queue���� ����
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_tail.Dequeue();
            if (i == cnt1 - 1) // ������ �ִ� �༮ Missing ó��
                f.Missing();
            else
                tmp.Enqueue(f);
        }

        friends_on_tail.Clear();

        int cnt2 = tmp.Count;
        // ���� ť �� ���� �� ģ���� ���� ��ġ ������;
        for (int i = 0; i < cnt2; i++)
        {
            Capybara_friend f = tmp.Dequeue();
            friends_on_tail.Enqueue(f);
        }

        tmp.Clear();

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

            if (i == cnt1 - 1)
            {
                f.Missing();
            }
            else
            {
                friends_on_head.Enqueue(f);
            }
        }

        StateCheck();
    }

    public void AllFriendJump()
    {
        IsTeamJumping = true;
        StartCoroutine(AllFriendJumpCoroutine());
    }
    private IEnumerator AllFriendJumpCoroutine()
    {
        foreach (Capybara_friend friend in friends_on_tail)
        {
            yield return new WaitForSeconds(jumpDelay);
            if (!friend.GetMissing())
                friend.Jump();
        }
        StartCoroutine(AllFriendJumpEndCheckCoroutine());
    }

    private IEnumerator AllFriendJumpEndCheckCoroutine()
    {
        bool allJumpingEnded = false;

        while (!allJumpingEnded)
        {
            allJumpingEnded = true;
            foreach (Capybara_friend friend in friends_on_tail)
            {
                if (!friend.IsJump())
                {
                    allJumpingEnded = false;
                    break;
                }
            }

            if (!allJumpingEnded)
            {
                // ģ������ ���� ���� ���̸� ��� ��ٷȴٰ� �ٽ� Ȯ���մϴ�.
                yield return new WaitForSeconds(0.1f);
            }
        }

        // ��� ģ������ ������ �Ϸ��ϸ� IsTeamJumping�� false�� �����մϴ�.
        IsTeamJumping = false;
    }

    public bool GetAllJumping()
    {
        return IsTeamJumping;
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
        tailSound.Play();
        friends_on_tail.Enqueue(capybara_friend);
        capybara_friend.PopFromHead();

        if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
            // ������ ���� ���� ��
            capybara_friend.transform.position = captain.GetComponent<Capybara_Move>().tailPosition.position + new Vector3(friendsOffset, 0, 0);
        else
            // �������� ���� ���� ��
            capybara_friend.transform.position = captain.GetComponent<Capybara_Move>().tailPosition.position + new Vector3(-friendsOffset, 0, 0);

        if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
        {
            capybara_friend.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            capybara_friend.GetComponent<SpriteRenderer>().flipX = false;
        }
        capybara_friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);

        StateCheck();
    }

    public void CaptainTaken() // ���� ī�ǹٶ� ����
    {

        foreach (Capybara_friend friend in friends_on_head) // �Ӹ��� �����ϴ� ī�ǹٶ�� ������
        {
            friend.Missing();

        }

        foreach (Capybara_friend friend in friends_on_tail) // ������ �����ϴ� ī�ǹٶ�� ������
        {
            friend.Missing();
        }
    }

    void StateCheck()
    {
        if (friends_on_tail.Count > 0) canRotate = false; // ȸ�� ����
        else canRotate = true;

        if (friends_on_head.Count > 0) canJump = false; // ���� ����
        else canJump = true;
    }

    public int HeadQueueCount()
    {
        return friends_on_head.Count;
    }
    public float HeadConstant()
    {
        return headConstant;
    }
    public float FriendOffset()
    {
        return friendsOffset;
    }

    public void FriendsMoveTrue()
    {
        foreach (Capybara_friend friend in friends_on_tail)
        {
            friend.GetComponent<Animator>().SetBool("isMove", true);
        }
    }
    public void FriendsMoveFalse()
    {
        foreach (Capybara_friend friend in friends_on_tail)
        {
            friend.GetComponent<Animator>().SetBool("isMove", false);
        }
    }

    public void JoinFriend()
    {
        float leastDistance = 10000f;
        Capybara_friend target = null;
        // ���� ����� ģ���� �շ���Ű��
        Capybara_friend[] friend = transform.GetComponentsInChildren<Capybara_friend>();
        for (int i = 0; i < friend.Length; i++)
        {

            if (leastDistance > friend[i].DistanceToCaptain()) // �ּҺ��� �� ������
            {
                if (friend[i].canJoin)
                {
                    target = friend[i];
                }
            }
        }

        if (target != null)
            target.JoinToGroup();
    }

    public void HeadFlip(bool LR)
    {
        foreach (Capybara_friend friend in friends_on_head)
        {
            friend.GetComponent<SpriteRenderer>().flipX = LR;
        }
    }

    public int TotalCapybaraFriendCount()
    {
        return friends_on_head.Count + friends_on_tail.Count;
    }

    public float DetectUpFloorCalculate()
    {
        return 1 + HeadQueueCount() * headConstant;
    }
}
