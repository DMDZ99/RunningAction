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

public class Item : MonoBehaviour
{
    public ItemType type;

    [SerializeField] private GameObject rushEffectPrefab;
    [SerializeField] private GameObject shieldEffectPrefab;
    [SerializeField] private GameObject magnetEffectPrefab;
    [SerializeField] private GameObject potionEffectPrefab;

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
            //case ItemType.Coin:
            //    GetCoin(player);
            //    break;
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

        // �����ڵ�
        // ���ֽ� ���� (3��)
        yield return new WaitForSeconds(3f);

        // ���� ����� 1�ʰ� ����
        yield return new WaitForSeconds(1f);

        //���������ڵ�

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

    //private void GetCoin(GameObject player)
    //{
    //    //��ȭ�� 50
    //    //��ȭ�� 10
    //    //��ȭ�� 1
    //}
}
