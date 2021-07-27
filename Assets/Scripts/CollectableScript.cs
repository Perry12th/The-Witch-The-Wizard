using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.2f;
    [SerializeField]
    private float frequency = 1.0f;

    // Position Storage Variables
    Vector3 posOffest = new Vector3();
    Vector3 tempPos = new Vector3();

    public void Start()
    {
        posOffest = transform.position;
    }

    public void Update()
    {
        tempPos = posOffest;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency);

        transform.position = tempPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
