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

    [SerializeField] private CoinPlacer coinPlacer;     // connect CoinPlacer

    [SerializeField] private Transform player;

    private void Start()
    {
        lastPosition = transform.position;  // ������ġ

        for (int i = 0; i < initialCount; i++)  // �ݺ�����
        {
            SpawnObstacle();
        }
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, lastPosition) < 20f)      // ������ ��ֹ���ġ�� 20�����پ��� ��ֹ� ���� �ݺ�
            return;

        SpawnObstacle();
    }


    private void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];    // �����տ��� ���� ����

        ObstacleData data = prefab.GetComponent<ObstacleData>();

        // ��������
        if (data.kind == ObstacleKind.Hole)
        {
            lastPosition += new Vector3(data.distanceToNext, 0f, 0f);   // ��ġ�� ����.
            lastKind = data.kind;
            return;
        }

        float gap = data.distanceToNext;    // ���� �ʱ�ȭ

        if (lastKind == ObstacleKind.Slide && data.kind == ObstacleKind.Slide)
        {
            gap = Mathf.Max(1f, gap); // 0�����ߴ��� �����̵���ֹ��� ��ħ -> �ּ� �Ÿ�
        }

        Vector3 prefabLocalY = prefab.transform.position;
        Vector3 position = lastPosition + new Vector3(gap, 0f, 0f); // ������ġ���
        position.y = prefabLocalY.y;    // �������� y��ǥ ���

        GameObject obstacle = Instantiate(prefab, position, Quaternion.identity, parent); // ��ֹ� ����

        if (data.kind == ObstacleKind.Jump)
            coinPlacer.PlaceCoinJump(obstacle.transform.position, data.coinYOffset);      // ���� �������� ��ֹ� ��ġ��
        else if (data.kind == ObstacleKind.Slide)
            coinPlacer.PlaceCoinSlide(obstacle.transform.position, data.coinYOffset);

            lastPosition = position;    // ��ġ, ���� ����
        lastKind = data.kind;

        ItemSpawner itemSpawner = FindObjectOfType<ItemSpawner>();
        if (itemSpawner != null)
        {
            itemSpawner.AddObstacle(obstacle.transform); // ��ֹ� ��ġ�� ������ �����ʿ� ����
        }
    }
}
