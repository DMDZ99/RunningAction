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
    [SerializeField] private float magnetRadius = 5f;   // 자석 범위

    //private bool isShieldActive = false;
    //private float shieldTimer = 0f;
    //private float invincibleTimer = 0f;

    //[SerializeField] private float shieldDuration = 30f;            // 실드 지속시간
    //[SerializeField] private float shieldInvincibleDuration = 1f;   // 실드 파괴후 무적시간

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

            Collider2D[] coins = Physics2D.OverlapCircleAll(playerTransform.position, magnetRadius);    // 자석 범위내에 모든 Collider2D 탐색

            foreach (var coin in coins)
            {
                if (coin.CompareTag("Coin"))    // 탐색한게 코인이면
                {
                    Vector3 direction = (playerTransform.position - coin.transform.position).normalized;    // 방향 = 플레이어위치 - 코인 위치
                    coin.transform.position += direction * magnetSpeed * Time.deltaTime;                    // 코인 위치를 방향만큼 자석속도로 가게함 시간동안
                }
            }

        }

        //if (!isShieldActive) return;
        
        //shieldTimer -= Time.deltaTime;      // 실드 타이머
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

        ApplyEffect(other.gameObject);                  // 아이템 효과 먼저 적용

        if (type == ItemType.Magnet)
        {
            GetComponent<SpriteRenderer>().enabled = false;     // 자석아이템일시 이미지를 비활성화
            GetComponent<Collider2D>().enabled = false;         // 충돌도 비활성화
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
            // 아이템 종류
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

        Debug.Log($"코인 : {coinType} + {score}");
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
        magnetTimer = magnetDuration;           // 타이머초기화
        playerTransform = player.transform;     // 플레이어 위치 저장
    }

    //private void ShieldPlayer(GameObject player)
    //{
    //    this.player = player.GetComponent<Player>();
    //    isShieldActive = true;
    //    shieldTimer = shieldDuration;

    //    if (this.player != null)
    //        this.player.isInvincible = false;

    //}

    //public void OnPlayerHitObstacle()       // 플레이어가 충돌시 실드 꺼지고 무적 켜지는 로직
    //{
    //    if (!isShieldActive) return;

    //    isShieldActive = false;    // 쉴드 해제

    //    if (player != null)
    //    {
    //        player.isInvincible = true;                    // 무적 켜기
    //        invincibleTimer = shieldInvincibleDuration;   // 무적 지속시간 타이머 초기화
    //    }
    //}
}