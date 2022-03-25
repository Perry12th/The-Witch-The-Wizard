using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField]
    private new ParticleSystem particleSystem;
    [SerializeField]
    private MeshRenderer meshRenderer;
    //private bool checkpointEnabled;

    // Start is called before the first frame update
    void Start()
    {
        disableCheckpoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.SetCheckpoint(this);
        }
    }

    public void enableCheckpoint()
    {
        particleSystem.Play();
        meshRenderer.material.EnableKeyword("_EMISSION");
    }

    public void disableCheckpoint()
    {
        particleSystem.Stop();
        meshRenderer.material.DisableKeyword("_EMISSION");
    }
    
}
