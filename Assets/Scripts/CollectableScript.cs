using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    //[SerializeField]
    //private float amplitude = 0.2f;
    [SerializeField]
    private float frequency = 1.0f;
    [SerializeField]
    private Conversation conversation;
    [SerializeField]
    private int candyAmount = 0;

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
            if (conversation != null && conversation.DialogueV2s.Length > 0)
            {
                other.GetComponent<WitcherScript>().SetDialogue(conversation);
            }
            if (candyAmount > 0)
            {
                other.GetComponent<WitcherScript>().GainCandy(candyAmount);
            }
            Destroy(gameObject);
        }
    }

}
