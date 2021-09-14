using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;
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
