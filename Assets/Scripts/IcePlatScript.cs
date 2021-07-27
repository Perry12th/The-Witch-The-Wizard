using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 40);
    }
}
