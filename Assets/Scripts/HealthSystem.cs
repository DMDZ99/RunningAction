using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;  // ü�¹� UI
    public float maxHealth = 500f;
    public float currentHealth;
    public float decreaseRate = 10f; // �ʴ� ���ҷ�

    private bool isDead = false;

    public Animator animator;
    public GameObject gameOverUI;
    public Text finalScoreText;  // ���� ���� ǥ�ÿ� �ؽ�Ʈ UI

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        // ü�� �ڿ� ����
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


        //  ���� ǥ��
        if (finalScoreText != null)
        {
            int finalScore = ScoreManager.Instance.GetScore();
            finalScoreText.text = "Final Score: " + finalScore.ToString();
        }

        // ���� ���� UI Ȱ��ȭ
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // ���� ���� ���� (�ʼ�)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �ٽ� �ε�
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
        //    Time.timeScale = 1f; // ���� ���� ���� (�ʼ�)
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �ٽ� �ε�
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
//            HealthSystem health = other.GetComponent<HealthSystem>(); //��ֹ� ������
//            if (health != null)
//            {
//                health.TakeDamage(damageAmount);
//            }
//        }
//    }
//}

