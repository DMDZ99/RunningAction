using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;  // 체력바 UI
    public float maxHealth = 500f;
    public float currentHealth;
    public float decreaseRate = 10f; // 초당 감소량

    private bool isDead = false;

    public Animator animator;
    public GameObject gameOverUI;
    public TextMeshProUGUI finalScoreText;  // 최종 점수 표시용 텍스트 UI 텍스트유징메시프로

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        TakeDamage(decreaseRate * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void UpdateHealthUI() // 헬스바
    {
        if (healthSlider != null)
        {
            Debug.Log(currentHealth);
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Game Over!");


        //  점수 표시
        if (finalScoreText != null)
        {
            int finalScore = ScoreManager.Instance.GetScore();
            finalScoreText.text = finalScore.ToString();
        }

        // 게임 오버 UI 활성화
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임 정지 해제 (필수)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }


}


