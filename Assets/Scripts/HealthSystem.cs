using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;  // 체력바 UI
    public float maxHealth = 500f;
    public float currentHealth;
    public float decreaseRate = 10f; // 초당 감소량

    private bool isDead = false;

    public Animator animator;
    public GameObject gameOverUI;
    public Text finalScoreText;  // 최종 점수 표시용 텍스트 UI

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        // 체력 자연 감소
        currentHealth -= decreaseRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
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

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
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
            finalScoreText.text = "Final Score: " + finalScore.ToString();
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

    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;
        private int currentScore = 0;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddScore(int amount)
        {
            currentScore += amount;
        }

        public int GetScore()
        {
            return currentScore;
        }

        public void ResetScore()
        {
            currentScore = 0;
        }

        //public void RestartGame()
        //{
        //    Time.timeScale = 1f; // 게임 정지 해제 (필수)
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
        //}
    }

}

//public class obstacle : MonoBehaviour
//{
//    public float damageAmount = 20f;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            HealthSystem health = other.GetComponent<HealthSystem>(); //장애물 데미지
//            if (health != null)
//            {
//                health.TakeDamage(damageAmount);
//            }
//        }
//    }
//}

