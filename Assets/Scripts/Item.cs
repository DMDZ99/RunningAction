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
        if (!other.CompareTag("Player")) return;        // �÷��̾ �ƴϸ� ����

        ApplyEffect(other.gameObject);                  // ������ ȿ������
        Destroy(gameObject);                            // �������� �浹�� �ı�
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

    private System.Collections.IEnumerator RushEffect(GameObject player)    // �ڷ�ƾ���� ó�� : ���ӽð�
    {
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d == null)
            yield break;

        float gameSpeed = rb2d.velocity.x;

        GameObject effect = null;   // ���ӿ�����Ʈ ����Ʈ
        if (rushEffectPrefab != null)
        {
            // �÷��̾���ġ�� ��������Ʈ����
            effect = Instantiate(rushEffectPrefab, player.transform.position, Quaternion.identity, player.transform);
        }

        rb2d.velocity = new Vector2(30, rb2d.velocity.y);   // �ӵ� ���� 30����

        // �����ڵ� : �÷��̾ ��ֹ� �浹�� �����Ѵ�. -> �÷��̾� �ʿ��� ���� ó��?

        // ���ֽ� ���� (3��)
        yield return new WaitForSeconds(3f);

        // ���� ���� = ���� �ӵ���
        rb2d.velocity = new Vector2(gameSpeed, rb2d.velocity.y);

        // ���� ����� 1�ʰ� ����
        yield return new WaitForSeconds(1f);

        //���������ڵ�

        // ����Ʈ �����
        if (effect != null)
            Destroy(effect);

    }

    //private System.Collections.IEnumerator ShieldEffect(GameObject player)
    //{
    //    // ��ȣ�� ���� = �浹������
    //    // �浹�� 1�ʰ� ����
    //    // ��ø�Ұ�? ����?
    //}

    //private System.Collections.IEnumerator MagnetEffect(GameObject player)
    //{
    //    // �ڼ� ȿ�� ���� (5��?)
    //}

    //private void HealPlayer(GameObject player)
    //{
    //    // �÷��̾��� ü�� ���� ȸ�� (�ִ밡 100�̸� 40����?)
    //    // �÷��̾� ü�� ���Ѽ� �����ʰ�
    //}

    private void GetCoin(GameObject player)
    {
        int score = 0;

        switch (coinType)
        {
            //��ȭ�� 1
            case CoinType.Copper:
                score = 1;
                break;
            //��ȭ�� 10
            case CoinType.Silver:
                score = 10;
                break;
            //��ȭ�� 50
            case CoinType.Gold:
                score = 50;
                break;
        }

        Debug.Log($"���� : {coinType} + {score}");
    }
}
