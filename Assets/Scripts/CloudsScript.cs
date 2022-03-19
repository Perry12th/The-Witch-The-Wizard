using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsScript : MonoBehaviour
{
    [SerializeField]
    MeshRenderer mr;

    private void Update()
    {
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 40);
    }
}
