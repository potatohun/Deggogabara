using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [Header("isFloorStuck")]
    public Transform headPosition;

    // ������Ʈ
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GroundCheck groundCheck;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        isJumping = false;
        isFloorStuck = false;
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
        if (DetectFloor())
            isFloorStuck = true;
        else
            isFloorStuck = false;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(inputVector.x , 0, 0); // ���� ������
    }

    public void OnMove(InputAction.CallbackContext value) // ������
    {
        if (FriendManager.friendManager.CanRotate())
        {
            animator.SetBool("isMove", true);
            FriendManager.friendManager.FriendsMoveTrue();
            // ȸ�� ���� (�ڿ� ī�ǹٶ� ģ���� ����)
            inputVector = value.ReadValue<Vector2>();
            inputVector *= movePower;

            if (inputVector.x < 0) // �� ���� ���
            {
                frontVector = Vector2.left;
                spriteRenderer.flipX = true;
                headPosition.transform.localPosition = new Vector3(0.98f, 0, 0);
                FriendManager.friendManager.HeadFlip(true);
                //transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (inputVector.x > 0)
            {
                frontVector = Vector2.right;
                spriteRenderer.flipX = false;
                headPosition.transform.localPosition = new Vector3(-0.98f, 0, 0);
                FriendManager.friendManager.HeadFlip(false);
                //transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                animator.SetBool("isMove", false);
                FriendManager.friendManager.FriendsMoveFalse();
            }
        }
        else
        {
            animator.SetBool("isMove", true);
            FriendManager.friendManager.FriendsMoveTrue();
            // ȸ�� �Ұ��� (�ڿ� ī�ǹٶ� ģ���� ����)
            if (value.ReadValue<Vector2>().x < 0) // ���� �Է��� ������ ��
            {
                if (frontVector.x < 0) // ���� �ٶ� ���� ����
                {
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else if (value.ReadValue<Vector2>().x > 0) // ������ �Է��� ������ ��
            {
                if (frontVector.x > 0) // ������ �ٶ� ���� ����
                {
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else // �Է��� ������ ������ �ֱ�
            {
                animator.SetBool("isMove", false);
                FriendManager.friendManager.FriendsMoveFalse();
                inputVector = Vector2.zero;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext value) // ����
    {
        if (value.performed)
        {
            if ((!isJumping) && (FriendManager.friendManager.CanJump())) // ���� ���� �ƴϰų�, ȸ���� �Ұ���(�ڿ� ģ���� ����) �ϸ� ���� ����
            {
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                FriendManager.friendManager.AllFriendJump();
            }
        }
    }

    bool DetectGround() // ���ٴ� ����
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, jumpDetectRange);
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Friends"))
            {
                return true; // Ground ���� O
            }
        }
        return false; // Ground ���� X
    }
    bool DetectFloor() // ���ٴ� ����
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up, (FriendManager.friendManager.HeadQueueCount()+1)* FriendManager.friendManager.HeadConstant());
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

    public bool GetFloorStuck()
    {
        return isFloorStuck;
    }
}
