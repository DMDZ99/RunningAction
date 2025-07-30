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

    private void Start()
    {
        lastPosition = transform.position;  // 시작위치

        for (int i = 0; i < initialCount; i++)  // 반복생성
        {
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];    // 프리팹에서 랜덤 선택

        ObstacleData data = prefab.GetComponent<ObstacleData>();

        // 구멍함정
        //if (data.soloObstacle)
        //{

        //}

        float gap = data.distanceToNext;    // 간격 초기화

        if (lastKind == ObstacleKind.Slide && data.kind == ObstacleKind.Slide)
        {
            gap = Mathf.Max(1.5f, data.distanceToNext); // 0으로했더니 슬라이드장애물이 겹침 -> 최소 거리
        }

        Vector3 position = lastPosition + new Vector3(gap, 0f, 0f); // 최종위치계산

        switch (data.kind)  // 장애물 종류에 따른 y값
        {
            case ObstacleKind.Jump:
                position.y = 3.5f;
                break;
            case ObstacleKind.Slide:
                position.y = 1f;
                break;
        }

        Instantiate(prefab, position, Quaternion.identity, parent); // 생성

        lastPosition = position;    // 위치, 종류 저장
        lastKind =data.kind;
    }
}
