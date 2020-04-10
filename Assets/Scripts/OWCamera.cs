using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWCamera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 2.5f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-.5f, 0, -100);    
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.TransformPoint(new Vector3(0, 0, target.position.z - 10));
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime, 75f);
        }
    }
}
