using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWallScript : MonoBehaviour
{

    [SerializeField]
    Material transparentM, opaqueM;

    [SerializeField]
    MeshRenderer meshR;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.material = transparentM;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            meshR.material = opaqueM;
        }
    }

}
