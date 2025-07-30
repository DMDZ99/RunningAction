using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static GameManager gameManager;

    public static GameManager Instance
    {
        get { return gameManager; }
    }

    private int currentScore = 0;
    UIManager uiManager;
    public UIManager UIManager
    {
        get { return uiManager; }
    }

    private void Awake()
    {
        gameManager = this;
    }
    private void Start()
    {
        //uiManager.UpdateScore(0);//점수 초기화 로직
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        //uiManager.SetRestart();//게임 재시작 로직

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        //uiManager.UpdateScore(currentScore);//점수증가로직
        Debug.Log("Score: " + currentScore);
    }

}

