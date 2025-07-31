using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private float itemSpawnTime = 30f;  // 아이템 스폰 시간
    [SerializeField] private float itemDistance = 10f;       // 플레이어 앞 몇 미터에 생성할지

    [SerializeField] private List<Transform> obstacleList;   // 장애물 위치 리스트
    [SerializeField] private List<Transform> coinList;       // 코인 위치 리스트 : 장애물과 겹쳤을때 생성안된걸 코인위치로 아이템 생성

    private float timer = 0f;
    private int currentItemIndex = 0;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // 플레이어 태그를 찾아서 트랜스폼가져옴
    }

    private void Update()
    {
        if (player == null || itemPrefabs.Count == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= itemSpawnTime)
        {
            bool canSpawn = CanSpawnItem();
            if (canSpawn)
            {
                SpawnNextItem();
                timer = 0f;
            }
            else
            {
                timer -= 1f;        // 아이템을 생성하지 못했을 때 생성 1초전으로 돌아감 (다시생성하기 위해)
                if (timer < 1f)
                    timer = 1f;     // 타이머 음수 방지, 1초 텀줘서 0초대에 아이템 계속생성못할때 기존쿨로 바로넘어가는거 방지
            }

        }
    }

    public void AddObstacle(Transform obstacle) // 장애물에서 장애물위치를 받음
    {
        obstacleList.Add(obstacle);
    }

    public void AddCoin(Transform coin)
    {
        coinList.Add(coin);
    }

    private void SpawnNextItem()
    {
        if (itemPrefabs.Count == 0)
            return;
        GameObject itemPrefab = itemPrefabs[currentItemIndex];  // 인덱스에 해당하는 아이템 생성
        Vector3 spawnPosition = player.transform.position + new Vector3(itemDistance, 0f, 0f);  // 스폰위치 = 플레이어 위치 + 아이템거리

        bool isTooClose = false;
        foreach (Transform obstacle in obstacleList)
        {
            if (Vector3.Distance(spawnPosition, obstacle.position) < 3f)
            {
                isTooClose = true;
                Debug.Log("장애물과 가까워 아이템 스폰 취소");
                break;
            }
        }

        if (isTooClose)
        {
            foreach (Transform coin in coinList)
            {
                bool coinGap = true;    // 코인과 장애물 간격 여부
                foreach (Transform obstacle in obstacleList)
                {
                    if (Vector3.Distance(coin.position, obstacle.position) < 3f)
                    {
                        coinGap = false;
                        break;
                    }
                }
                
                if (coinGap)
                {
                    Instantiate(itemPrefab, coin.position, Quaternion.identity);    // 코인 위치에 아이템 생성
                    Debug.Log("코인 위치에 아이템 다시 생성");
                    currentItemIndex = (currentItemIndex + 1) % itemPrefabs.Count;
                    return;
                }
            }
            Debug.Log("생성 가능한 코인 위치가 없음");
            return;
        }

        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        currentItemIndex = (currentItemIndex + 1) % itemPrefabs.Count;  // 현재 인덱스끝나면 다음 인덱스로
    }

    private bool CanSpawnItem()     // 스폰 가능 여부
    {
        Vector3 spawnPosition = player.position + new Vector3(itemDistance, 0f, 0f);

        foreach (Transform obstacle in obstacleList)
        {
            if (Vector3.Distance(spawnPosition, obstacle.position) < 3f)
                return false;  // 너무 가까우면 생성 불가
        }
        return true; // 생성 가능
    }

}
