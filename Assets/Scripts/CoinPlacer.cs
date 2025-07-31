using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> coinPrefabs;
    [SerializeField] private Transform coinParent;

    [SerializeField] private float coinSpacing = 1f;    // ���� ����
    [SerializeField] private int coinCount = 5;         // ��ֹ��ϳ��� ���� ���� = 1

    public void PlaceCoinJump(Vector3 obstaclePosition, float coinYOffset)    // jumpObstacle's coin
    {
        for(int i = 0; i < coinCount; i++)
        {
            Vector3 coinposition = obstaclePosition + new Vector3(i * coinSpacing, coinYOffset, 0);   // coin place
            GameObject prefab = GetCoinLine();
            Instantiate(prefab, coinposition, Quaternion.identity, coinParent);
        }
    }

    public void PlaceCoinSlide(Vector3 obstaclePosition, float coinYOffset)   // slideObstacle's coin
    {
        for (int i = 0;i < coinCount; i++)
        {
            Vector3 coinposition = obstaclePosition + new Vector3(i * coinSpacing, coinYOffset, 0);   // coin place
            GameObject prefab = GetCoinLine();
            Instantiate(prefab, coinposition, Quaternion.identity, coinParent);
        }
    }

    private int coinIndex = 0;
    private GameObject GetCoinLine()    // ���� ������� (������ �ʹ� ���Ұ� £��)
    {
        coinIndex++;

        if (coinIndex % 20 == 0)
            return coinPrefabs[2];  // 20������ ��ȭ
        else if (coinIndex % 5 == 0)
            return coinPrefabs[1];  // 5������ ��ȭ
        else
            return coinPrefabs[0];  // ����Ʈ ��ȭ
    }
}
