using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // 여기서
    [SerializeField] public float forwardSpeed;
    [SerializeField] public float JumpForce;
    [SerializeField] public float invincibleTime; // 무적 시간
    [SerializeField] public LayerMask platformLayer;
    [SerializeField] public Animator animator;
    [SerializeField] public new Rigidbody2D rigidbody2D;
    [SerializeField] public Collider2D slideCollider;
    [SerializeField] public Collider2D runnerCollider;
    [SerializeField] public SpriteRenderer playerOriginSprite;

    [SerializeField] public int HP = 100; //나중에 삭제해야 한다

    public int jumpCount = 0;
    public float extraSpeed; // 추가 스피드

    public bool isInvincible; // 무적을 판단하는 변수
    public bool isGrounded; // 땅에 닿았는지 확인하는 변수
    public bool isJumping; // 점프를 하고 있는지 확인하는 변수

    public bool controlsEnabled = true;
    // 여기까지 가지고옴;

    static GameManager gameManager;

    public static GameManager Instance
    {
        get; private set;
    }
    [Header("Other Managers")]
    public UIManager UIManager;
    public SoundManager SoundManager;
    //public StageManager StageManager;//스테이지 추가시
    public Player Player;

    [Header("Game States")]
    public bool isGameOver = false;
    //public bool isPaused = false;
    public int totalScore = 0;
    public int maxHearts = 3;

    // 게임 속도 제어 (예: 점점 빨라지는 런 게임)
    [Header("Gameplay")]
    public float gameSpeed = 5f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 10f;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
        if (isGameOver) return;

        // 게임 속도 증가
        //gameSpeed += speedIncreaseRate * Time.deltaTime;
        //gameSpeed = Mathf.Min(gameSpeed, maxSpeed);
        gameSpeed = Mathf.Min(gameSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);
        SetRunSpeed(gameSpeed);
    }

    public void StartGame()
    {
        isGameOver = false;
        //isPaused = false;//일시정지
        totalScore = 0;
        //currentHearts = maxHearts; //현재 체력 초기화
        gameSpeed = 5f;

        //Player.ResetState();
        //Player.EnableControls();

        //UIManager 에서 게임화면 상단의 정보창
        //UIManager 화면에 표시되는 점수를 0으로 초기화
        //UIManager.UpdateHearts(currentHearts); // 하트값 점수

        //Player.EnableControls();// 캐릭터 입력
        //StageManager.BeginStage();//사운드 매니저 시작음 
        //SoundManager.PlayBGM(); //플레이어 사운드
    }
    // 게임 일시정지
    //public void PauseGame()
    //{
    //    isPaused = true;
    //    Time.timeScale = 0f;
    //   // UIManager.ShowPausePanel(); //사운드 정지 옵션
    //   // SoundManager.PlayPauseSound(); //플레이어 정지 사운드 
    //}
    // 게임 재시작 
    //public void ResumeGame()
    //{
    //    isPaused = false;
    //    Time.timeScale = 1f;
    //    UIManager.HidePausePanel();
    //}

    public void GameOver()
    {
        isGameOver = true;
        Player.DisableControls(); //게임 동작 정지
        //StageManager.StopStage(); //스테이지 정지
        //SoundManager.PlayGameOverSound(); //게임오버 사운드 삽입
        // UIManager.ShowGameOverUI(); //게임오버UI출력
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
        SceneManager.GetActiveScene().name);
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        //UIManager.UpdateScore(totalScore);
    }

    public void DamagePlayer(float damage)
    {
        //currentHearts -= damage;//현재 하트수에서 피해 감산
        //currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);//체력이 음수로 떨어지거나 최대치를 안넘도록 제한

        // UIManager.UpdateHealth(playerHealth); 체력바 업데이트

        //if (playerHealth <= 0f && !isGameOver) //게임오버 로직
        //{
        //    GameOver();
        //}
    }

    // 여기서

    /* Player 기본 이동 메소드 */
    public void RunnerMovementMethod()
    {
        Vector3 baseForwardSpeed = rigidbody2D.velocity;
        baseForwardSpeed.x = forwardSpeed + extraSpeed; // 추가 스피드로 Rush스피드를 제어할 수 있다.

        rigidbody2D.velocity = baseForwardSpeed;
    }

    public void RunnerJumpMethod()
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

    public void RunnerSlideMethod()
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

    /* 장애물 충돌 시 구현된는 애니메이션에 필요한 메소드*/

    public void SpriteDamageMethod() // 출동 시 알파 값 조정
    {
        animator.SetTrigger("isDamege");

        Color color = playerOriginSprite.color;
        color.a = 0.5f;
        playerOriginSprite.color = color;
        // 무적 true
        isInvincible = true;
    }
    public void SpriteResetMethod() // 출동 후 알파 값 회복
    {
        Color color = playerOriginSprite.color;
        color.a = 1f;
        playerOriginSprite.color = color;
        // 무적 false
        isInvincible = false;
    }
    public void StartSuperRushMethod() // rush가 시작되는 메소드
    {
        animator.SetBool("isRush", true);
        isInvincible = true;
        extraSpeed = 6;
    }
    public void EndSuperRushMethod() // rush가 끝나는 메소드
    {
        animator.SetBool("isRush", false);
        isInvincible = false;
        extraSpeed = 0;
    }
    public void TakeDamage(int amt) // 이름을 동사로 시작하도록
    {
        HP -= amt;
        if (HP <= 0)
        {
            animator.SetTrigger("isDeath");
            // GameOver 페널
            // 다리기 멈추기
            controlsEnabled = false;
            // ...
        }
    }
    public void SetRunSpeed(float speed) => forwardSpeed = speed;

    // 여기까지 가지고 옴
}

