using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;  // ü�¹� UI
    public float maxHealth = 500f;
    public float currentHealth;
    public float decreaseRate = 10f; // �ʴ� ���ҷ�

    private bool isDead = false;

    public Animator animator;
    public GameObject gameOverUI;
    public TextMeshProUGUI finalScoreText;  // ���� ���� ǥ�ÿ� �ؽ�Ʈ UI �ؽ�Ʈ��¡�޽�����

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
    
    void UpdateHealthUI() // �ｺ��
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


        //  ���� ǥ��
        if (finalScoreText != null)
        {
            int finalScore = ScoreManager.Instance.GetScore();
            finalScoreText.text = finalScore.ToString();
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


}


