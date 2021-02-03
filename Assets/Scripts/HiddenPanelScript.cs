using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPanelScript : MonoBehaviour
{
    public MeshRenderer mr;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mr.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mr.enabled = true;
        }
    }
}
