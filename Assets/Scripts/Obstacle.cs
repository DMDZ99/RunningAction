using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs;
    public Transform parent;
    public int initialCount = 5;   // 게임 시작시 생성할 장애물 수

    private Vector3 lastPosition;
    private ObstacleKind lastKind = ObstacleKind.Jump;  // 마지막장애물 종류 = 점프

    [SerializeField] private float spawnCooldown = 0.5f;  // 장애물생성쿨타임
    private float spawnObstacleTimer = 0f;

    [SerializeField] private CoinPlacer coinPlacer;     // connect CoinPlacer

    [SerializeField] private Transform player;
    //[SerializeField] private float destroyDistance = 30f;   // 지나가면 파괴되게

    private void Start()
    {
        lastPosition = transform.position;  // 시작위치

        for (int i = 0; i < initialCount; i++)  // 반복생성
        {
            SpawnObstacle();
        }
    }

    private void Update()
    {
        spawnObstacleTimer += Time.deltaTime;

        if (spawnObstacleTimer < spawnCooldown)
            return;

        SpawnObstacle();
        spawnObstacleTimer = 0f;
    }


    private void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];    // 프리팹에서 랜덤 선택

        ObstacleData data = prefab.GetComponent<ObstacleData>();

        // 구멍함정
        if (data.kind == ObstacleKind.Hole)
        {
            lastPosition += new Vector3(data.distanceToNext, 0f, 0f);   // 위치만 비운다.
            lastKind = data.kind;
            return;
        }

        float gap = data.distanceToNext;    // 간격 초기화

        if (lastKind == ObstacleKind.Slide && data.kind == ObstacleKind.Slide)
        {
            gap = Mathf.Max(3f, gap); // 0으로했더니 슬라이드장애물이 겹침 -> 최소 거리
        }

        if (lastKind == ObstacleKind.Slide && data.kind == ObstacleKind.Jump)
        {
            gap = Mathf.Min(10f, gap); // 슬라이드 뒤에 점프나올때 간격 더 벌리기
        }


            Vector3 prefabLocalY = prefab.transform.position;
        Vector3 position = lastPosition + new Vector3(gap, 0f, 0f); // 최종위치계산
        position.y = prefabLocalY.y;    // 프리팹의 y좌표 사용

        GameObject obstacle = Instantiate(prefab, position, Quaternion.identity, parent); // 장애물 생성

        if (data.kind == ObstacleKind.Jump)
            coinPlacer.PlaceCoinJump(obstacle.transform.position, data.coinYOffset);      // 코인 종류별로 장애물 위치에
        else if (data.kind == ObstacleKind.Slide)
            coinPlacer.PlaceCoinSlide(obstacle.transform.position, data.coinYOffset);

            lastPosition = position;    // 위치, 종류 저장
        lastKind = data.kind;

        ItemSpawner itemSpawner = FindObjectOfType<ItemSpawner>();
        if (itemSpawner != null)
        {
            itemSpawner.AddObstacle(obstacle.transform); // 장애물 위치를 아이템 스포너에 전달
        }
    }
}
