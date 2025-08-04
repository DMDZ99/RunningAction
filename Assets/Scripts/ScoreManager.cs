using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        // ΩÃ±€≈Ê ∆–≈œ
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString("D3");
    }

    public int GetScore()
    {
        return score;
    }
}


//public class PlayerController : MonoBehaviour
//{
//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("coin"))
//        {
//            Destroy(other.gameObject);
//        }
//    }
//}
