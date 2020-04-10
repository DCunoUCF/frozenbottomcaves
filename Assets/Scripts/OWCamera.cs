using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWCamera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-.5f, 0, -10);    // node 0's location and -10 to not clip through the scene
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.TransformPoint(new Vector3(0, 0, target.position.z - 10));

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime, 500f);
        }
    }
}
