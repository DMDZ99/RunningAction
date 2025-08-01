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

    // ���⵵ �ּ�
    [SerializeField] private GameObject rushEffectPrefab;
    [SerializeField] private GameObject shieldEffectPrefab;
    [SerializeField] private GameObject magnetEffectPrefab;
    [SerializeField] private GameObject potionEffectPrefab;

    public CoinType coinType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;        // Ignore if not player

        // �÷��̾�� �������� ������ ���ӸŴ����� �˷��ֱ�
        GameManager gameManager = FindObjectOfType<GameManager>();
        // ���ӸŴ����� ������ Ÿ�Թ޴°� �ʿ�
        
        ApplyEffect(other.gameObject);                  // Apply item
        Destroy(gameObject);                            // Destory item after collision
    }



    private void ApplyEffect(GameObject player)
    {
        switch (type)
        {
            // ���⼭ �ּ�ó��
            case ItemType.Rush:
                RushPlayer(player);
                break;
            //case ItemType.Shield:
            //    ShieldPlayer(player);
            //    break;
            //case ItemType.Magnet:
            //    MagnetPlayer(player);
            //    break;
            //case ItemType.Potion:
            //    HealPlayer(player);
            //    break;
            //������ �ּ�X
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

    private void RushPlayer(GameObject player)
    {

    }
}