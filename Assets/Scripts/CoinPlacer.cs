using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> coinPrefabs;
    [SerializeField] private Transform coinParent;

    [SerializeField] private float coinSpacing = 1f;    // 코인 간격
    [SerializeField] private int coinCount = 5;         // 장애물하나당 코인 개수 = 1

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
    private GameObject GetCoinLine()    // 코인 순서대로 (랜덤은 너무 운빨요소가 짙음)
    {
        coinIndex++;

        if (coinIndex % 20 == 0)
            return coinPrefabs[2];  // 20번마다 금화
        else if (coinIndex % 5 == 0)
            return coinPrefabs[1];  // 5번마다 은화
        else
            return coinPrefabs[0];  // 디폴트 동화
    }
}
