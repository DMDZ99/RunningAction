using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float JumpForce;
    //[SerializeField] private float invincibleTime; // 무적 시간
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private Collider2D slideCollider;
    [SerializeField] private Collider2D runnerCollider;
    [SerializeField] private SpriteRenderer playerOriginSprite;
    [SerializeField] private HealthSystem healthSystem;

    //[SerializeField] private int HP = 100; //나중에 삭제해야 한다

    private int jumpCount = 0;
    private float extraSpeed; // 추가 스피드

    private bool isShielded; // 실드가 작동하고 있는지 확인하는 변수
    private bool isInvincible; // 무적을 판단하는 변수
    private bool isGrounded; // 땅에 닿았는지 확인하는 변수
    private bool isJumping; // 점프를 하고 있는지 확인하는 변수

    private bool controlsEnabled = true;
    //public event Action OnHitObstacle;
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
        baseForwardSpeed.x = forwardSpeed + extraSpeed; // 추가 스피드로 Rush스피드를 제어할 수 있다.

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

        if (isJumping == true)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                AddGravity();
                Invoke("SubtractGravity", 0.5f);
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
        if (collision.CompareTag("Obstacle"))
        {
            // damage 시작점 무적 판단 (무적 시 리턴) 해당 예외 처리는 장애물에만 필요하다.
            if (isInvincible == true)
                return;

            if (isShielded)
            {
                ActiveShield();
                Invoke("InactiveShield", 0.5f);
                //InactiveShield();
                CancelInvoke("EndShield");
                return;
            }

            // HP 깎이는 메소드 추가해야 함 (HP == Time)
            // 죽었을 때 게임 종료 및 다시시작 페널이 뜸
            // 게임 종료 로직: 1. 타임 아웃; 2. 데미지를 받아서 시간 전부 소진
            // TakeDamager(100); (Player 스크립트에 있는 메서드)
            healthSystem.TakeDamage(100);

            SpriteDamageMethod();
            Invoke("SpriteResetMethod", 2);
        }

        // Shield 로직:
        // 1. 아이템을 먹으면 활성하고 기본 무적 시간이 있다 무적 시간이 끝나면 정상으로 돌아온다.
        // 2. 활성 도중 장애물과 충돌하면 바로 무적 타임이 끝난다. (별도에 애니메이션이 필요하다)
        if (collision.CompareTag("ShieldItem"))
        {
            Debug.Log(collision.tag);
            Destroy(collision.gameObject);
            StartShield();
            Invoke("EndShield", 5);            
        }

        //if (collision.CompareTag("Coin"))
        //{
        //    Debug.Log(collision.tag);

        //    Destroy(collision.gameObject);
        //    // 점수가 늘어나는 메소드 추가해야함
        //}

        if (collision.CompareTag("RushItem")) 
        {
            Debug.Log(collision.tag);

            Destroy(collision.gameObject);
            StartSuperRushMethod();
            Invoke("EndSuperRushMethod", 5);
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
    private void StartSuperRushMethod() // rush가 시작되는 메소드
    {
        animator.SetBool("isRush", true);
        isInvincible = true;
        extraSpeed = 6;
    }
    private void EndSuperRushMethod() // rush가 끝나는 메소드
    {
        animator.SetBool("isRush", false);
        isInvincible = false;
        extraSpeed = 0;
    }

    private void StartShield()
    {
        animator.SetBool("isShield", true); // 실드가 있는 상태로 튀기 (기본 무적 시간 보유)
        isShielded = true;
    }
    private void EndShield()
    {
        animator.SetBool("isShield", false);
        isShielded = false;
    }
    private void ActiveShield()
    {
        animator.SetBool("isShieldActive", true);
    }
    private void InactiveShield()
    {
        animator.SetBool("isShieldActive", false);
        animator.SetBool("isShield", false);
        isShielded = false;
    }

    private void AddGravity()
    {
        rigidbody2D.gravityScale = 20;
    }
    private void SubtractGravity()
    {
        rigidbody2D.gravityScale = 4;
    }

    //private void TakeDamager(int amt)
    //{
    //    HP -= amt;
    //    if (HP <= 0)
    //    {
    //        animator.SetTrigger("isDeath");

    //        controlsEnabled = true;
    //    }
    //}
}
