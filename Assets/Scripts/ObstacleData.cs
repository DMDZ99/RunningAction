using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleKind { Jump, Slide }//, Hole
public class ObstacleData : MonoBehaviour
{
    public ObstacleKind kind = ObstacleKind.Jump;

    public float distanceToNext = 10f;   // ���� ��ֹ����� �Ÿ�����

    public float obstacleYPosition;     // ��ֹ� Y ��ġ (�������� �ٸ��� �ֱ����� ����)

    public float coinYOffset;           // ��ֹ��� ���� ����

    // public bool soloObstacle = false;   // �ܵ�������ֹ� ( ���� )
}
