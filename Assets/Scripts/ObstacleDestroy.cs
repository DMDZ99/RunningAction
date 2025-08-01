using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float destroyDistance = 50f;   // �������� �ı��ǰ�


    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");  // ������ �������� �±�ã�����ְ�
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (player.position.x - destroyDistance > transform.position.x) // ��ֹ���ġ�� �÷��̾���ġ���� �ı��Ÿ����� �� �����϶� �ı�
        {
            Destroy(gameObject);
        }
    }
}
