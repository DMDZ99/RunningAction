using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    public ObstacleKind kind = Obstacle.Jump;

    public float distanceToNext = 5f;   // 다음 장애물과의 거리간격

    // public bool soloObstacle = false;   // 단독생성장애물 ( 구멍 )
}
