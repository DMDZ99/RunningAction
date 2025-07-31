using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;

    [SerializeField] private float itemSpawnTime = 30f;  // ������ ���� �ð�
    [SerializeField] private float itemDistance = 10f;       // �÷��̾� �� �� ���Ϳ� ��������

    [SerializeField] private List<Transform> obstacleList;   // ��ֹ� ��ġ ����Ʈ
    [SerializeField] private List<Transform> coinList;       // ���� ��ġ ����Ʈ : ��ֹ��� �������� �����ȵȰ� ������ġ�� ������ ����

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
            bool canSpawn = CanSpawnItem();
            if (canSpawn)
            {
                SpawnNextItem();
                timer = 0f;
            }
            else
            {
                timer -= 1f;        // �������� �������� ������ �� ���� 1�������� ���ư� (�ٽû����ϱ� ����)
                if (timer < 1f)
                    timer = 1f;     // Ÿ�̸� ���� ����, 1�� ���༭ 0�ʴ뿡 ������ ��ӻ������Ҷ� ������� �ٷγѾ�°� ����
            }

        }
    }

    public void AddObstacle(Transform obstacle) // ��ֹ����� ��ֹ���ġ�� ����
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
        GameObject itemPrefab = itemPrefabs[currentItemIndex];  // �ε����� �ش��ϴ� ������ ����
        Vector3 spawnPosition = player.transform.position + new Vector3(itemDistance, 0f, 0f);  // ������ġ = �÷��̾� ��ġ + �����۰Ÿ�

        bool isTooClose = false;
        foreach (Transform obstacle in obstacleList)
        {
            if (Vector3.Distance(spawnPosition, obstacle.position) < 3f)
            {
                isTooClose = true;
                Debug.Log("��ֹ��� ����� ������ ���� ���");
                break;
            }
        }

        if (isTooClose)
        {
            foreach (Transform coin in coinList)
            {
                bool coinGap = true;    // ���ΰ� ��ֹ� ���� ����
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
                    Instantiate(itemPrefab, coin.position, Quaternion.identity);    // ���� ��ġ�� ������ ����
                    Debug.Log("���� ��ġ�� ������ �ٽ� ����");
                    currentItemIndex = (currentItemIndex + 1) % itemPrefabs.Count;
                    return;
                }
            }
            Debug.Log("���� ������ ���� ��ġ�� ����");
            return;
        }

        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        currentItemIndex = (currentItemIndex + 1) % itemPrefabs.Count;  // ���� �ε��������� ���� �ε�����
    }

    private bool CanSpawnItem()     // ���� ���� ����
    {
        Vector3 spawnPosition = player.position + new Vector3(itemDistance, 0f, 0f);

        foreach (Transform obstacle in obstacleList)
        {
            if (Vector3.Distance(spawnPosition, obstacle.position) < 3f)
                return false;  // �ʹ� ������ ���� �Ұ�
        }
        return true; // ���� ����
    }

}
