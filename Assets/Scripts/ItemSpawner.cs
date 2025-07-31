using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private float itemSpawnTime = 30f;  // ������ ���� �ð�
    [SerializeField] private float itemDistance = 10f;       // �÷��̾� �� �� ���Ϳ� ��������

    private float timer = 0f;
    private int currentItemIndex = 0;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // �÷��̾� �±׸� ã�Ƽ� Ʈ������������
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
        GameObject itemPrefab = itemPrefabs[currentItemIndex];  // �ε����� �ش��ϴ� ������ ����
        Vector3 spawnPosition = player.transform.position + new Vector3(itemDistance, 0f, 0f);  // ������ġ = �÷��̾� ��ġ + �����۰Ÿ�
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        currentItemIndex = (currentItemIndex + 1) % itemPrefabs.Count;  // ���� �ε��������� ���� �ε�����
    }
}
