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

    // 컴포넌트
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
        if (DetectGround())// 땅바닥 감지 (점프 중 확인)
            isJumping = false;
        else
            isJumping = true;
    }
    void FixedUpdate()
    {
        transform.position += new Vector3(inputVector.x , 0, 0); // 실제 움직임
    }

    public void OnMove(InputAction.CallbackContext value) // 움직임
    {
        if (FriendManager.friendManager.CanRotate())
        {
            // 회전 가능 (뒤에 카피바라 친구들 없음)
            inputVector = value.ReadValue<Vector2>();
            inputVector *= movePower;

            if (inputVector.x < 0) // 앞 방향 계산
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
            // 회전 불가능 (뒤에 카피바라 친구들 있음)
            if (value.ReadValue<Vector2>().x < 0) // 왼쪽 입력이 들어왔을 때
            {
                if (frontVector.x < 0) // 왼쪽 바라볼 때만 적용
                {
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else if (value.ReadValue<Vector2>().x > 0) // 오른쪽 입력이 들어왔을 때
            {
                if (frontVector.x > 0) // 오른쪽 바라볼 때만 적용
                {
                    inputVector = value.ReadValue<Vector2>();
                    inputVector *= movePower;
                }
            }
            else // 입력이 없으면 가만히 있기
                inputVector = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext value) // 점프
    {
        if (value.performed)
        {
            if ((!isJumping) && (FriendManager.friendManager.CanJump())) // 점프 중이 아니거나, 회전이 불가능(뒤에 친구들 존재) 하면 점프 가능
            {
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                FriendManager.friendManager.AllFriendJump();
            }
        }
    }

    bool DetectGround() // 땅바닥 감지
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, jumpDetectRange);
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider.CompareTag("Ground"))
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
}
