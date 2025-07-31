using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleKind { Jump, Slide }//, Hole
public class ObstacleData : MonoBehaviour
{
    public ObstacleKind kind = ObstacleKind.Jump;

    public float distanceToNext = 10f;   // 다음 장애물과의 거리간격

    public float obstacleYPosition;     // 장애물 Y 위치 (종류마다 다른값 넣기위해 생성)

    public float coinYOffset;           // 장애물과 코인 간격

    // public bool soloObstacle = false;   // 단독생성장애물 ( 구멍 )
}
