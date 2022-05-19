using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraScript : MonoBehaviour, ICharmable, IDamagable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int charmLimit = 3;
    private int timesCharmed;
    private bool isSuperCharmed;
    private bool witchInShop;
    public void ApplyCharm()
    {
        timesCharmed++;
        if (timesCharmed >= charmLimit)
        {
            animator.SetTrigger("Super Charamed");
        }
        else
        {
            animator.SetTrigger("Charmed");
        }
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        animator.SetTrigger("Attacked");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            witchInShop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            witchInShop = false;
        }
    }

    private void CheckStatus()
    {
        if (!isSuperCharmed && witchInShop)
        {
            animator.SetTrigger("Angry");
        }
    }
}
