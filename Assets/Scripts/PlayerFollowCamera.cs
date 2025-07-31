using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    private float offsetX;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            return;

        offsetX = transform.position.x - target.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 targetPos = new Vector3(target.position.x + offsetX, transform.position.y, -10);

        transform.position = targetPos;
    }
}
