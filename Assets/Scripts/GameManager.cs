using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
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
        Player.SetRunSpeed(gameSpeed);
    }

    public void StartGame()
    {
        isGameOver = false;
        //isPaused = false;//일시정지
        totalScore = 0;
        //currentHearts = maxHearts; //현재 체력 초기화
        gameSpeed = 5f;

        Player.ResetState();
        Player.EnableControls();

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



}

