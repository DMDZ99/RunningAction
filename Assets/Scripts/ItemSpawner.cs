using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private float itemSpawnTime = 30f;  // 아이템 스폰 시간
    [SerializeField] private float itemDistance = 10f;       // 플레이어 앞 몇 미터에 생성할지

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
            SpawnNextItem();
            timer = 0f;
        }
    }

    private void SpawnNextItem()
    {
        if (itemPrefabs.Count == 0)
            return;
        GameObject itemPrefab = itemPrefabs[currentItemIndex];  // 인덱스에 해당하는 아이템 생성
        Vector3 spawnPosition = player.transform.position + new Vector3(itemDistance, 0f, 0f);  // 스폰위치 = 플레이어 위치 + 아이템거리
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        currentItemIndex = (currentItemIndex + 1) % itemPrefabs.Count;  // 현재 인덱스끝나면 다음 인덱스로
    }
}
