using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Rigidbody2D followedBody;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(followedBody.position.x, transform.position.y, -10f) + Vector3.right * 7, 0.5f);
    }
}
