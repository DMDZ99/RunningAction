using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float invincibleTime; // 무적 시간
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private Collider2D slideCollider;
    [SerializeField] private Collider2D runnerCollider;
    [SerializeField] private SpriteRenderer playerOriginSprite;

    [SerializeField] private int HP = 100; //나중에 삭제해야 한다

    private int jumpCount = 0;
    private float extraSpeed; // 추가 스피드

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

        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log(collision.tag);
            if (collision.CompareTag("Obstacle"))
            {
                // HP 깎이는 메소드 추가해야 함
                HP -= 10;

                SpriteDamageMethod();
                Invoke("SpriteResetMethod", invincibleTime);
            }
        }

        if (collision.CompareTag("Coin"))
        {
            Debug.Log(collision.tag);
            if (collision.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);
                // 점수가 늘어나는 메소드 추가해야함
            }
        }

        if (collision.CompareTag("RushItem"))
        {
            Debug.Log(collision.tag);
            if (collision.CompareTag("RushItem"))
            {
                Destroy(collision.gameObject);
                StartSuperRushMethod();
                Invoke("EndSuperRushMethod", invincibleTime = 5);
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
}
