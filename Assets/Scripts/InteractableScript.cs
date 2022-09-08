using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class InteractableScript : MonoBehaviour
{
    [SerializeField]
    protected TextMeshPro interactionText;
    [SerializeField]
    protected Collider interactionCollider;
    protected bool isInteractable;

    public abstract void Interact();

    protected void Start()
    {
        interactionText.enabled = false;
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteractable = true;
            //interactionText.enabled = true;
        }
    }

    protected void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteractable = false;
            interactionText.enabled = false;
        }
    }

}
