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

    // 컴포넌트
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
        if (groundCheck.IsGround())// 땅바닥 감지 (점프 중 확인)
        {
            isJumping = false;
            animator.SetBool("isJump", false);
        }
        else
        {
            isJumping = true;
            animator.SetBool("isJump", true);
        }

        if (frontCheck.IsGround())// 전방 벽 감지
        {
            isFrontStuck = true;
        }
        else
        {
            isFrontStuck = false;
        }

        // 위에 벽 확인
        if (DetectUpFloor())
            isFloorStuck = true;
        else
            isFloorStuck = false;

        // 뒤에 벽 확인
        if (DetectBackFloor())
            isBackStuck = true;
        else
            isBackStuck = false;
    }

    void FixedUpdate()
    {
        if (isFrontStuck)
        {
            if ((frontVector.x > 0) && (inputVector.x > 0)) // 오른쪽 쳐다볼 때
            {
                // 못감
            }
            else if ((frontVector.x < 0) && (inputVector.x < 0)) // 왼쪽 쳐다볼 때
            {
                // 못감
            }
            else
            {
                transform.position += new Vector3(inputVector.x, 0, 0); // 실제 움직임
            }

        }
        else
        {
            transform.position += new Vector3(inputVector.x, 0, 0); // 실제 움직임
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
    public void OnMove(InputAction.CallbackContext value) // 움직임
    {
        if (FriendManager.friendManager.CanRotate())
        {
            animator.SetBool("isMove", true);

            // 회전 가능 (뒤에 카피바라 친구들 없음)
            inputVector = value.ReadValue<Vector2>();
            inputVector *= movePower;

            if (inputVector.x < 0) // 앞 방향 계산
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
        else // 회전이 불가능 할때
        {
            // 회전 불가능 (뒤에 카피바라 친구들 있음)
            if (value.ReadValue<Vector2>().x < 0) // 왼쪽 입력이 들어왔을 때
            {
                if (frontVector.x < 0) // 왼쪽 바라볼 때만 적용
                {
                    animator.SetBool("isMove", true);
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else if (value.ReadValue<Vector2>().x > 0) // 오른쪽 입력이 들어왔을 때
            {
                if (frontVector.x > 0) // 오른쪽 바라볼 때만 적용
                {
                    animator.SetBool("isMove", true);
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else // 입력이 없으면 가만히 있기
            {
                animator.SetBool("isMove", false);
                inputVector = Vector2.zero;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext value) // 점프
    {
        if (value.performed)
        {
            if ((!isFloorStuck) && (!isJumping) && (FriendManager.friendManager.CanJump())) // 점프 중이 아니거나, 회전이 불가능(뒤에 친구들 존재) 하면 점프 가능
            {
                jumpSound.Play();
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                FriendManager.friendManager.AllFriendJump();
            }
        }
    }

    bool DetectUpFloor() // 땅바닥 감지
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(headPosition.position, Vector2.up, FriendManager.friendManager.DetectUpFloorCalculate());
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true; // Ground 감지 O
            }
        }
        return false; // Ground 감지 X
    }

    bool DetectBackFloor() // 땅바닥 감지
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -frontVector, FriendManager.friendManager.FriendOffset() + 2f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true; // Ground 감지 O
            }
        }
        return false; // Ground 감지 X
    }

    bool DetectFrontFloor() // 땅바닥 감지
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, frontVector, FriendManager.friendManager.FriendOffset());
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true; // Ground 감지 O
            }
        }
        return false; // Ground 감지 X
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
