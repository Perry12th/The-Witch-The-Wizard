using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    void Start()
    {
        offset = gameObject.transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = target.position + offset;
    }
}
