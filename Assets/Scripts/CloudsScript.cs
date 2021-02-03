using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RotateClouds", 0, 0.025f);
    }

    // Update is called once per frame
    void RotateClouds()
    {
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 50);
    }
}
