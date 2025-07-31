using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;  // 체력바 UI
    public float maxHealth = 100f;
    public float currentHealth;
    public float decreaseRate = 5f; // 초당 감소량

    private bool isDead = false;

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
        // 게임 오버 처리 여기서 추가 (씬 전환 등)
    }
}

public class obstacle : MonoBehaviour
{
    public float damageAmount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>(); //장애물 데미지
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
