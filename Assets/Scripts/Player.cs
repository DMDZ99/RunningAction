using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Collider2D slideCollider;

    private Animator animator;
    private Collider2D runnerCollider;
    private int jumpCount = 0;

    private bool isGrounded; // 땅에 닿았는지 확인하는 변수
    private bool isJumping; // 점프를 하고 있는지 확인하는 변수

    private bool controlsEnabled = true;
    public event Action OnHitObstacle;
    public void SetRunSpeed(float speed) => forwardSpeed = speed;
    public void EnableControls() => controlsEnabled = true;
    public void DisableControls() => controlsEnabled = false;
    public void ResetState()
    {
        jumpCount = 0;
        isJumping = false;
        animator.SetBool("isJump", false);
        animator.SetBool("isSlide", false);
        slideCollider.enabled = false;
        runnerCollider.enabled = true;
    }
    // Start is called before the first frame update
    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        runnerCollider = GetComponent<BoxCollider2D>(); //스크립트가 있는 오브젝트에 코라이더를 받아온다.
    }


    public void Start()
    {
       // animator = GetComponentInChildren<Animator>();
        //runnerCollider = GetComponent<BoxCollider2D>(); //스크립트가 있는 오브젝트에 코라이더를 받아온다.
    }

    // Update is called once per frame
    public void Update()
    {
        if (!controlsEnabled) return;
        // 앞으로 가는 로직
        RunnerMovementMethod();

        //점프 로직
        RunnerJumpMethod();

        // 슬라이드 로직
        RunnerSlideMethod();
    }

    
    private void FixedUpdate()//고정으로 0.2초 마다 실행한다.
    {
        // 땅에 닿았는지 확인하는 방법 raycast, 에디터 상에서만 Ray를 그려주는 함수
        Debug.DrawRay(rigidbody2D.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigidbody2D.position, Vector3.down, 2.0f, platformLayer);

        if (rayHit.collider != null && rigidbody2D.velocity.y < 0)
        {
            isGrounded = rayHit.distance < 1.05f;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && isJumping)
        {
            animator.SetBool("isJump", false);
            isJumping = false;
            jumpCount = 0;
        }
    }

    private void RunnerMovementMethod()
    {
        Vector3 baseForwardSpeed = rigidbody2D.velocity;
        baseForwardSpeed.x = forwardSpeed;

        rigidbody2D.velocity = baseForwardSpeed;
    }

    private void RunnerJumpMethod()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (Mathf.Abs(rigidbody2D.velocity.y) < 0.01f) // Mathf.Abs 절대값을 구할 때 사용한다
        //    {
        //        jumpCount = 1;
        //    }
        //    else
        //    {
        //        jumpCount++;
        //    }

        //    if (jumpCount <= 2)
        //    {
        //        animator.SetBool("isJump", true);
        //        rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        //        isJumping = true; // 점프를 실행하면 bool 값 isJumping은 참
        //    }

        //    // rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); 기본 점프 로직
        //    // ForceMode2D.Impulse 좀 더 현실적인 점프를 구현하는 코드
        //    // && rigidbody2D.velocity.y == 0 무한 점프를 방지하기위한 조건식 (하지만 실패했다)
        //    // && Mathf.Approximately(rigidbody2D.velocity.y, 0) 무한 점프를 방지하기위한 조건식 (하지만 실패했다)
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                jumpCount = 1;
            else
                jumpCount++;

            if (jumpCount <= 2)
            {
                animator.SetBool("isJump", true);
                rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    private void RunnerSlideMethod()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("isSlide", true);
            slideCollider.enabled = true; // component를 끄고 키고하는 코드.
            runnerCollider.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isSlide", false);
            slideCollider.enabled = false;
            runnerCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            // Debug.Log(collision.tag);
            if (collision.CompareTag("Obstacle"))
                OnHitObstacle?.Invoke();
        }
    }
}
