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
    [SerializeField] private float invincibleTime; // 무적 시간
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D runnerCollider;
    [SerializeField] private SpriteRenderer playerOriginSprite;

    private int jumpCount = 0;

    private bool isInvincible; // 무적을 판단하는 변수
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
        //animator = GetComponentInChildren<Animator>(); // 자녀에 있는 component를 가지고 올 때 GetComponentInChildren사용
        //runnerCollider = GetComponent<BoxCollider2D>(); //스크립트가 있는 오브젝트에 코라이더를 받아온다.
        //playerOriginSprite = GetComponentInChildren<SpriteRenderer>(); // SpriteRenderer의 초기값을 저장한다
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

    // 장애물 충돌 시 구현된는 애니메이션
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // damage 시작점 무적 판단 (무적 시 리턴)
        if (isInvincible == true)
            return;
        // 무적이라는 기능은 없다 지금 메소드에서는 isInvincible가 참일 때 충돌하는 코드를 건너 뛰어서 return 하게 만든거다.

        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log(collision.tag);
            if (collision.CompareTag("Obstacle"))
            {
                SpriteDamageMethod();
                Invoke("SpriteResetMethod", invincibleTime);

                //animator.SetTrigger("isDamege");
                // bool은 실행과 끝을 다 체크해야 할때, trigger는 실행만 할 때 (돌아가는 transition을 부착해야한다)
                // condition이 없으면 애니메이션이 끝나고 바로 다음동작 실행

                // Color color = playerOriginSprite.color;
                //color.a = 0.5f;
                //playerOriginSprite.color = color;
                // 색갈+무적
                // Invoke로 다시돌아가는 로직 구현 (시간턴을 주는 코드)
                // Invoke의 사용법 [ Invoke("함수이름", 딜레이_초); ] 이제 알파 값을 조절하는 함수를 구현해야한다.
            }
        }
        else if (collision.CompareTag("Coin"))
        {
            Debug.Log(collision.tag);
            if (collision.CompareTag("Coin"))
            {
                
            }
        }

    }
    // 장애물 충돌 시 구현된는 애니메이션에 필요한 메소드
    private void SpriteDamageMethod() // 출동 시 알파 값 조정
    {
        animator.SetTrigger("isDamege");

        Color color = playerOriginSprite.color;
        color.a = 0.5f;
        playerOriginSprite.color = color;
        // 무적 true
        isInvincible = true;
    }
    private void SpriteResetMethod() // 출동 후 알파 값 회복
    {
        Color color = playerOriginSprite.color;
        color.a = 1f;
        playerOriginSprite.color = color;
        // 무적 false
        isInvincible = false;
    }
}
