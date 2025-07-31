using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleKind { Jump, Slide }//, Hole
public class ObstacleData : MonoBehaviour
{
    public ObstacleKind kind = ObstacleKind.Jump;

    public float distanceToNext = 10f;   // 다음 장애물과의 거리간격

    // public bool soloObstacle = false;   // 단독생성장애물 ( 구멍 )
}
