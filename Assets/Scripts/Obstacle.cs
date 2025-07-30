using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public List<GameObject> obstaclePrefabs;
    public Transform parent;
    public int initialCount = 5;   // ���� ���۽� ������ ��ֹ� ��

    private Vector3 lastPosition;
    private ObstacleKind lastKind = ObstacleKind.Jump;  // ��������ֹ� ���� = ����

    private void Start()
    {
        lastPosition = transform.position;  // ������ġ

        for (int i = 0; i < initialCount; i++)  // �ݺ�����
        {
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];    // �����տ��� ���� ����

        ObstacleData data = prefab.GetComponent<ObstacleData>();

        // ��������
        //if (data.soloObstacle)
        //{

        //}

        float gap = data.distanceToNext;    // ���� �ʱ�ȭ

        if (lastKind == ObstacleKind.Slide && data.kind == ObstacleKind.Slide)
        {
            gap = Mathf.Max(1.5f, data.distanceToNext); // 0�����ߴ��� �����̵���ֹ��� ��ħ -> �ּ� �Ÿ�
        }

        Vector3 position = lastPosition + new Vector3(gap, 0f, 0f); // ������ġ���

        switch (data.kind)  // ��ֹ� ������ ���� y��
        {
            case ObstacleKind.Jump:
                position.y = 3.5f;
                break;
            case ObstacleKind.Slide:
                position.y = 1f;
                break;
        }

        Instantiate(prefab, position, Quaternion.identity, parent); // ����

        lastPosition = position;    // ��ġ, ���� ����
        lastKind =data.kind;
    }
}
