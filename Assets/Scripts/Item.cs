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
        if (!other.CompareTag("Player")) return;        // 플레이어가 아니면 리턴

        ApplyEffect(other.gameObject);                  // 아이템 효과적용
        Destroy(gameObject);                            // 아이템은 충돌후 파괴
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

    private System.Collections.IEnumerator RushEffect(GameObject player)    // 코루틴으로 처리 : 지속시간
    {
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d == null)
            yield break;

        float gameSpeed = rb2d.velocity.x;

        GameObject effect = null;   // 게임오브젝트 이펙트
        if (rushEffectPrefab != null)
        {
            // 플레이어위치에 질주이펙트생성
            effect = Instantiate(rushEffectPrefab, player.transform.position, Quaternion.identity, player.transform);
        }

        rb2d.velocity = new Vector2(30, rb2d.velocity.y);   // 속도 임의 30증가

        // 무적코드 : 플레이어가 장애물 충돌을 무시한다. -> 플레이어 쪽에서 무적 처리?

        // 질주시 무적 (3초)
        yield return new WaitForSeconds(3f);

        // 질주 종료 = 원래 속도로
        rb2d.velocity = new Vector2(gameSpeed, rb2d.velocity.y);

        // 질주 종료시 1초간 무적
        yield return new WaitForSeconds(1f);

        //무적해제코드

        // 이펙트 사라짐
        if (effect != null)
            Destroy(effect);

    }

    //private System.Collections.IEnumerator ShieldEffect(GameObject player)
    //{
    //    // 보호막 적용 = 충돌전까지
    //    // 충돌시 1초간 무적
    //    // 중첩불가? 가능?
    //}

    //private System.Collections.IEnumerator MagnetEffect(GameObject player)
    //{
    //    // 자석 효과 적용 (5초?)
    //}

    //private void HealPlayer(GameObject player)
    //{
    //    // 플레이어의 체력 일정 회복 (최대가 100이면 40정도?)
    //    // 플레이어 체력 상한선 넘지않게
    //}

    private void GetCoin(GameObject player)
    {
        int score = 0;

        switch (coinType)
        {
            //동화는 1
            case CoinType.Copper:
                score = 1;
                break;
            //은화는 10
            case CoinType.Silver:
                score = 10;
                break;
            //금화는 50
            case CoinType.Gold:
                score = 50;
                break;
        }

        Debug.Log($"코인 : {coinType} + {score}");
    }
}
