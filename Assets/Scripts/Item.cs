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

    [SerializeField] private GameObject rushEffectPrefab;
    [SerializeField] private GameObject shieldEffectPrefab;
    [SerializeField] private GameObject magnetEffectPrefab;
    [SerializeField] private GameObject potionEffectPrefab;

    public CoinType coinType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;        // Ignore if not player

        ApplyEffect(other.gameObject);                  // Apply item
        Destroy(gameObject);                            // Destory item after collision
    }

    private void ApplyEffect(GameObject player)
    {
        switch (type)
        {
            case ItemType.Rush:
                StartCoroutine(RushEffect(player));
                break;
            //case ItemType.Shield:
            //    StartCoroutine(ShieldEffect(player));
            //    break;
            //case ItemType.Magnet:
            //    StartCoroutine(MagnetEffect(player));
            //    break;
            //case ItemType.Potion:
            //    HealPlayer(player);
            //    break;
            case ItemType.Coin:
                GetCoin(player);
                break;
        }
    }

    private System.Collections.IEnumerator RushEffect(GameObject player)    // Coroutine : for duration time
    {
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d == null)
            yield break;

        float gameSpeed = rb2d.velocity.x;

        GameObject effect = null;
        if (rushEffectPrefab != null)
        {
            // Instantiate rush effect at player's position
            effect = Instantiate(rushEffectPrefab, player.transform.position, Quaternion.identity, player.transform);
        }

        rb2d.velocity = new Vector2(30, rb2d.velocity.y);   // Temporarily increase speed to 30

        // Invincibility: Player ignores obstacle collisions -> Player.cs

        // Invincible during rush (3 seconds)
        yield return new WaitForSeconds(3f);

        // End rush: revert to original speed
        rb2d.velocity = new Vector2(gameSpeed, rb2d.velocity.y);

        // 1 second of invincibility after rush ends
        yield return new WaitForSeconds(1f);

        //Remove invincibility

        // Destroy effect
        if (effect != null)
            Destroy(effect);

    }

    //private System.Collections.IEnumerator ShieldEffect(GameObject player)
    //{
    //    // Apply shield effect until collision
    //    // 1 second of invincibility after shield breaks
    //    // Stack or not Stack?
    //}

    //private System.Collections.IEnumerator MagnetEffect(GameObject player)
    //{
    //    // Apply magnet effect for 5 seconds
    //}

    //private void HealPlayer(GameObject player)
    //{
    //    // Heal player (player MaxHp's 30% or 40%?)
    //    currentHearts += (int)(maxHearts * 0.3);


    //    // Do not exceed max HP 
    //    if (currentHearts >= maxHearts)
    //        currentHearts = maxHearts;
    //}

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

        Debug.Log($"ÄÚÀÎ : {coinType} + {score}");
    }
}