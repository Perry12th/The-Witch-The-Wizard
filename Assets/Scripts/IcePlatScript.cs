using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatScript : MonoBehaviour
{
    public void Update()
    {
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 40);
    }

    public void DestroyPlatform()
    {
        Destroy(gameObject);
    }
}
