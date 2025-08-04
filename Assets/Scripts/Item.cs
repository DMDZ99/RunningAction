using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Rush,
    Shield,
    Magnet,
    Potion,
    Coin
}

public enum CoinType
{
    Copper,
    Silver,
    Gold
}

public class Item : MonoBehaviour
{
    public ItemType type;

    //// Effect
    //[SerializeField] private GameObject rushEffectPrefab;
    //[SerializeField] private GameObject shieldEffectPrefab;
    //[SerializeField] private GameObject magnetEffectPrefab;
    //[SerializeField] private GameObject potionEffectPrefab;

    public CoinType coinType;

    private Player player;

    private bool isMagnetActive = false;
    private float magnetTimer = 0f;
    private Transform playerTransform;

    [SerializeField] private float magnetDuration = 5f;
    [SerializeField] private float magnetSpeed = 10f;
    [SerializeField] private float magnetRadius = 5f;   // �ڼ� ����

    //private bool isShieldActive = false;
    //private float shieldTimer = 0f;
    //private float invincibleTimer = 0f;

    //[SerializeField] private float shieldDuration = 30f;            // �ǵ� ���ӽð�
    //[SerializeField] private float shieldInvincibleDuration = 1f;   // �ǵ� �ı��� �����ð�

    private void Update()
    {
        if (isMagnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if(magnetTimer <= 0f)
            {
                isMagnetActive = false;
                return;
            }

            Collider2D[] coins = Physics2D.OverlapCircleAll(playerTransform.position, magnetRadius);    // �ڼ� �������� ��� Collider2D Ž��

            foreach (var coin in coins)
            {
                if (coin.CompareTag("Coin"))    // Ž���Ѱ� �����̸�
                {
                    Vector3 direction = (playerTransform.position - coin.transform.position).normalized;    // ���� = �÷��̾���ġ - ���� ��ġ
                    coin.transform.position += direction * magnetSpeed * Time.deltaTime;                    // ���� ��ġ�� ���⸸ŭ �ڼ��ӵ��� ������ �ð�����
                }
            }

        }

        //if (!isShieldActive) return;
        
        //shieldTimer -= Time.deltaTime;      // �ǵ� Ÿ�̸�
        //if (shieldTimer <= 0f)
        //{
        //    isShieldActive = false;
        //}

        //if (invincibleTimer > 0f)
        //{
        //    invincibleTimer -= Time.deltaTime;
        //    if (invincibleTimer <= 0f && player != null)
        //    {
        //        player.isInvincible = false;
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;        // Ignore if not player

        ApplyEffect(other.gameObject);                  // ������ ȿ�� ���� ����

        if (type == ItemType.Magnet)
        {
            GetComponent<SpriteRenderer>().enabled = false;     // �ڼ��������Ͻ� �̹����� ��Ȱ��ȭ
            GetComponent<Collider2D>().enabled = false;         // �浹�� ��Ȱ��ȭ
        }

        else
        {
            Destroy(gameObject);                            // Destory item after collision
        }
    }



    private void ApplyEffect(GameObject player)
    {
        switch (type)
        {
            // ������ ����
            //case ItemType.Rush:
            //    RushPlayer(player);
            //    break;
            //case ItemType.Shield:
            //    ShieldPlayer(player);
            //    break;
            case ItemType.Magnet:
                MagnetPlayer(player);
                break;
            case ItemType.Potion:
                HealPlayer(player);
                break;
            case ItemType.Coin:
                GetCoin(player);
                break;
        }
    }

    private void GetCoin(GameObject player)
    {
        int score = 0;

        switch (coinType)
        {
            //Coppercoin = 1score
            case CoinType.Copper:
                score = 1;
                break;
            //Silvercoin = 10score
            case CoinType.Silver:
                score = 10;
                break;
            //Goldcoin = 50score
            case CoinType.Gold:
                score = 50;
                break;
        }

        Debug.Log($"���� : {coinType} + {score}");
    }

    //private void RushPlayer(GameObject player)
    //{
            
    //}

    private void HealPlayer(GameObject player)
    {
        // Heal player (player MaxHp's 30% or 40%?)
        HealthSystem healthSystem = player.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            float healAmount = healthSystem.maxHealth * 0.3f;
            healthSystem.currentHealth += healAmount;
        }

        // Do not exceed max HP 
        if (healthSystem.currentHealth >= healthSystem.maxHealth)
            healthSystem.currentHealth = healthSystem.maxHealth;
    }

    private void MagnetPlayer(GameObject player)
    {
        isMagnetActive = true;
        magnetTimer = magnetDuration;           // Ÿ�̸��ʱ�ȭ
        playerTransform = player.transform;     // �÷��̾� ��ġ ����
    }

    //private void ShieldPlayer(GameObject player)
    //{
    //    this.player = player.GetComponent<Player>();
    //    isShieldActive = true;
    //    shieldTimer = shieldDuration;

    //    if (this.player != null)
    //        this.player.isInvincible = false;

    //}

    //public void OnPlayerHitObstacle()       // �÷��̾ �浹�� �ǵ� ������ ���� ������ ����
    //{
    //    if (!isShieldActive) return;

    //    isShieldActive = false;    // ���� ����

    //    if (player != null)
    //    {
    //        player.isInvincible = true;                    // ���� �ѱ�
    //        invincibleTimer = shieldInvincibleDuration;   // ���� ���ӽð� Ÿ�̸� �ʱ�ȭ
    //    }
    //}
}