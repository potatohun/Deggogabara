using UnityEngine;
using UnityEngine.InputSystem;

public class Capybara_Move : MonoBehaviour
{
    [SerializeField]
    [Header("Front")]
    Vector2 frontVector = Vector2.right;

    [SerializeField]
    [Header("Input Vector")]
    Vector2 inputVector;

    [SerializeField]
    [Header("Move Power")]
    float movePower = 0.25f;

    [SerializeField]
    [Header("Jump Power")]
    float jumpPower = 30f;

    [SerializeField]
    [Header("Jump Detect Range")]
    float jumpDetectRange = 0.8f;

    [SerializeField]
    [Header("isJumping")]
    bool isJumping;

    [SerializeField]
    [Header("isFloorStuck")]
    bool isFloorStuck;

    [SerializeField]
    [Header("isBackStuck")]
    bool isBackStuck;

    [SerializeField]
    [Header("isBackStuck")]
    bool isFrontStuck;

    [SerializeField]
    [Header("Falling")]
    bool falling;

    public Transform headPosition;
    public Transform tailPosition;
    public Transform orange;

    public AudioSource jumpSound;

    // ������Ʈ
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GroundCheck groundCheck;
    public GroundCheck frontCheck;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isJumping = false;
        isFloorStuck = false;
        isFrontStuck = false;
        falling = false;
        frontVector = Vector2.right;
    }

    private void Update()
    {
        if (groundCheck.IsGround())// ���ٴ� ���� (���� �� Ȯ��)
        {
            isJumping = false;
            animator.SetBool("isJump", false);
        }
        else
        {
            isJumping = true;
            animator.SetBool("isJump", true);
        }

        if (frontCheck.IsGround())// ���� �� ����
        {
            isFrontStuck = true;
        }
        else
        {
            isFrontStuck = false;
        }

        // ���� �� Ȯ��
        if (DetectUpFloor())
            isFloorStuck = true;
        else
            isFloorStuck = false;

        // �ڿ� �� Ȯ��
        if (DetectBackFloor())
            isBackStuck = true;
        else
            isBackStuck = false;
    }

    void FixedUpdate()
    {
        if (isFrontStuck)
        {
            if ((frontVector.x > 0) && (inputVector.x > 0)) // ������ �Ĵٺ� ��
            {
                // ����
            }
            else if ((frontVector.x < 0) && (inputVector.x < 0)) // ���� �Ĵٺ� ��
            {
                // ����
            }
            else
            {
                transform.position += new Vector3(inputVector.x, 0, 0); // ���� ������
            }

        }
        else
        {
            transform.position += new Vector3(inputVector.x, 0, 0); // ���� ������
        }
    }

     
    public float GetMovePower()
    {
        return movePower;
    }
    public void SetMovePower(float power)
    {
        movePower = power;
    }
    public void OnMove(InputAction.CallbackContext value) // ������
    {
        if (FriendManager.friendManager.CanRotate())
        {
            animator.SetBool("isMove", true);

            // ȸ�� ���� (�ڿ� ī�ǹٶ� ģ���� ����)
            inputVector = value.ReadValue<Vector2>();
            inputVector *= movePower;

            if (inputVector.x < 0) // �� ���� ���
            {
                frontVector = Vector2.left;
                spriteRenderer.flipX = true;
                headPosition.localPosition = new Vector3(0.3f, 1, 0);
                tailPosition.localPosition = new Vector3(1.3f, -0.25f, 0);
                orange.localPosition = new Vector3(-0.9827635f, 1.682442f, 0);
                frontCheck.gameObject.transform.localPosition = new Vector3(-2.26f, -0.1f, 0);
                FriendManager.friendManager.HeadFlip(true);
                //transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (inputVector.x > 0)
            {
                frontVector = Vector2.right;
                spriteRenderer.flipX = false;
                headPosition.localPosition = new Vector3(-0.3f, 1, 0);
                tailPosition.localPosition = new Vector3(-1.3f, -0.25f, 0);
                orange.localPosition = new Vector3(0.9827635f, 1.682442f, 0);
                frontCheck.gameObject.transform.localPosition = new Vector3(2.26f, -0.1f, 0);
                FriendManager.friendManager.HeadFlip(false);
                //transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                animator.SetBool("isMove", false);
            }
        }
        else // ȸ���� �Ұ��� �Ҷ�
        {
            // ȸ�� �Ұ��� (�ڿ� ī�ǹٶ� ģ���� ����)
            if (value.ReadValue<Vector2>().x < 0) // ���� �Է��� ������ ��
            {
                if (frontVector.x < 0) // ���� �ٶ� ���� ����
                {
                    animator.SetBool("isMove", true);
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else if (value.ReadValue<Vector2>().x > 0) // ������ �Է��� ������ ��
            {
                if (frontVector.x > 0) // ������ �ٶ� ���� ����
                {
                    animator.SetBool("isMove", true);
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else // �Է��� ������ ������ �ֱ�
            {
                animator.SetBool("isMove", false);
                inputVector = Vector2.zero;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext value) // ����
    {
        if (value.performed)
        {
            if ((!isFloorStuck) && (!isJumping) && (FriendManager.friendManager.CanJump())) // ���� ���� �ƴϰų�, ȸ���� �Ұ���(�ڿ� ģ���� ����) �ϸ� ���� ����
            {
                jumpSound.Play();
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                FriendManager.friendManager.AllFriendJump();
            }
        }
    }

    bool DetectUpFloor() // ���ٴ� ����
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(headPosition.position, Vector2.up, FriendManager.friendManager.DetectUpFloorCalculate());
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true; // Ground ���� O
            }
        }
        return false; // Ground ���� X
    }

    bool DetectBackFloor() // ���ٴ� ����
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -frontVector, FriendManager.friendManager.FriendOffset() + 2f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true; // Ground ���� O
            }
        }
        return false; // Ground ���� X
    }

    bool DetectFrontFloor() // ���ٴ� ����
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, frontVector, FriendManager.friendManager.FriendOffset());
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true; // Ground ���� O
            }
        }
        return false; // Ground ���� X
    }

    public Vector2 GetFront()
    {
        return frontVector;
    }

    public bool GetJuming()
    {
        return isJumping;
    }

    public bool GetUpFloorStuck()
    {
        return isFloorStuck;
    }
    public bool GetBackFloorStuck()
    {
        return isBackStuck;
    }
    public bool GetFalling()
    {
        return falling;
    }
}
