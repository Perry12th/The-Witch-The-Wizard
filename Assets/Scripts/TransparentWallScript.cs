using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWallScript : MonoBehaviour
{

    [SerializeField]
    private Material transparentMat, opaqueMat;
    [SerializeField]
    private List<MeshRenderer> meshRenderers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.material = transparentMat;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.material = opaqueMat;
            }
        }
    }

}
