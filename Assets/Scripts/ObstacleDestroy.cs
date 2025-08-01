using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestroy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float destroyDistance = 50f;   // 지나가면 파괴되게


    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");  // 각각의 프리팹이 태그찾을수있게
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        if (player.position.x - destroyDistance > transform.position.x) // 장애물위치가 플레이어위치에서 파괴거리보다 더 왼쪽일때 파괴
        {
            Destroy(gameObject);
        }
    }
}
