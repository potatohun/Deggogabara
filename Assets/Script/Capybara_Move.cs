using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class Capybara_Move : MonoBehaviour
{
    [SerializeField]
    [Header("Front")]
    Vector2 frontVector;

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

    // ������Ʈ
    private Rigidbody2D rigidbody;
    private SpriteRenderer sp;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        isJumping = false;
        frontVector = Vector2.left;
    }

    private void Update()
    {
        if (DetectGround())// ���ٴ� ���� (���� �� Ȯ��)
            isJumping = false;
        else
            isJumping = true;
    }
    void FixedUpdate()
    {
        transform.position += new Vector3(inputVector.x , 0, 0); // ���� ������
    }

    public void OnMove(InputAction.CallbackContext value) // ������
    {
        if (FriendManager.friendManager.CanRotate())
        {
            // ȸ�� ���� (�ڿ� ī�ǹٶ� ģ���� ����)
            inputVector = value.ReadValue<Vector2>();
            inputVector *= movePower;

            if (inputVector.x < 0) // �� ���� ���
            {
                frontVector = Vector2.left;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else if (inputVector.x > 0)
            {
                frontVector = Vector2.right;
                transform.localScale = new Vector3(1.5f, -1.5f, 1.5f);
            }
        }
        else
        {
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
                inputVector = Vector2.zero;
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
            if(hit.collider.CompareTag("Ground"))
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
}
