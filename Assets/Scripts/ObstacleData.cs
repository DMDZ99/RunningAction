using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    public ObstacleKind kind = Obstacle.Jump;

    public float distanceToNext = 5f;   // ���� ��ֹ����� �Ÿ�����

    // public bool soloObstacle = false;   // �ܵ�������ֹ� ( ���� )
}
