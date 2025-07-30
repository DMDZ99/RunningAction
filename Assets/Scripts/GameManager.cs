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
    //public StageManager StageManager;//�������� �߰���
    public Player Player;

    [Header("Game States")]
    public bool isGameOver = false;
    //public bool isPaused = false;
    public int totalScore = 0;
    public int maxHearts = 3;

    // ���� �ӵ� ���� (��: ���� �������� �� ����)
    [Header("Gameplay")]
    public float gameSpeed = 5f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 10f;

    private void Awake()
    {
        // �̱��� �ʱ�ȭ
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

        // ���� �ӵ� ����
        gameSpeed += speedIncreaseRate * Time.deltaTime;
        gameSpeed = Mathf.Min(gameSpeed, maxSpeed);
    }

    public void StartGame()
    {
        isGameOver = false;
        //isPaused = false;//�Ͻ�����
        totalScore = 0;
        //currentHearts = maxHearts; //���� ü�� �ʱ�ȭ
        gameSpeed = 5f;

        //UIManager ���� ����ȭ�� ����� ����â
        //UIManager ȭ�鿡 ǥ�õǴ� ������ 0���� �ʱ�ȭ
        //UIManager.UpdateHearts(currentHearts); // ��Ʈ�� ����

        //Player.EnableControls();// ĳ���� �Է�
        //StageManager.BeginStage();//���� �Ŵ��� ������ 
        //SoundManager.PlayBGM(); //�÷��̾� ����
    }
    // ���� �Ͻ�����
    //public void PauseGame()
    //{
    //    isPaused = true;
    //    Time.timeScale = 0f;
    //   // UIManager.ShowPausePanel(); //���� ���� �ɼ�
    //   // SoundManager.PlayPauseSound(); //�÷��̾� ���� ���� 
    //}
    // ���� ����� 
    //public void ResumeGame()
    //{
    //    isPaused = false;
    //    Time.timeScale = 1f;
    //    UIManager.HidePausePanel();
    //}

    public void GameOver()
    {
        isGameOver = true;
        //Player.DisableControls(); //���� ���� ����
        //StageManager.StopStage(); //�������� ����
        //SoundManager.PlayGameOverSound(); //���ӿ��� ���� ����
        // UIManager.ShowGameOverUI(); //���ӿ���UI���
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
        //currentHearts -= damage;//���� ��Ʈ������ ���� ����
        //currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);//ü���� ������ �������ų� �ִ�ġ�� �ȳѵ��� ����

        // UIManager.UpdateHealth(playerHealth); ü�¹� ������Ʈ

        //if (playerHealth <= 0f && !isGameOver) //���ӿ��� ����
        //{
        //    GameOver();
        //}
    }



}

