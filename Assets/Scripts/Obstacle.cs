using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float highPosY = 1f;     // ��ֹ���ġ Y����Ѽ�
    public float lowPosY = -1f;     // ���Ѽ�

    public Transform topObject;     // ��ֹ� ���
    public Transform bottomObject;  // ��ֹ� �ϴ�

    public float widthPadding = 4f; // ��ֹ��� x�� ����

    public Vector3 SetRandomPlace(Vector3 lastPosition, int obstacleCount)  // ��ֹ� ���� ��ġ
    {
        // �����ֹ�, �ϴ���ֹ� ��ġ ���� ������
        // �����ֹ� ������ �ϴ���ֹ��� ���� x������ ������ �������� ��������
        topObject.position = new Vector3(0, 8f);        // ��ġ ���� �����ؾ���
        bottomObject.position = new Vector3(0, 4f);

        if ( topObject.position.x == bottomObject.position.x)
        {
            topObject.position.y = null;
        }

        Vector3 placePosition = lastPosition + new Vector3(widthPadding, 0);    // x��
        placePosition.y = Random.Range(lowPosY, highPosY);                      // y�� �� ����

        transform.position = placePosition;

        return placePosition;   // ��ȯ
    }
}
