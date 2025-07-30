using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float highPosY = 1f;     // 장애물위치 Y축상한선
    public float lowPosY = -1f;     // 하한선

    public Transform topObject;     // 장애물 상단
    public Transform bottomObject;  // 장애물 하단

    public float widthPadding = 4f; // 장애물간 x축 간격

    public Vector3 SetRandomPlace(Vector3 lastPosition, int obstacleCount)  // 장애물 랜덤 배치
    {
        // 상단장애물, 하단장애물 위치 각각 설정후
        // 상단장애물 생성시 하단장애물도 같은 x값으로 생성시 일정간격 벌어지게
        topObject.position = new Vector3(0, 8f);        // 수치 따로 조정해야함
        bottomObject.position = new Vector3(0, 4f);

        if ( topObject.position.x == bottomObject.position.x)
        {
            topObject.position.y = null;
        }

        Vector3 placePosition = lastPosition + new Vector3(widthPadding, 0);    // x축
        placePosition.y = Random.Range(lowPosY, highPosY);                      // y축 값 랜덤

        transform.position = placePosition;

        return placePosition;   // 반환
    }
}
