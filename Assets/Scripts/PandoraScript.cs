using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraScript : MonoBehaviour, ICharmable, IDamagable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerSpotter playerSpotter;
    [SerializeField]
    private int charmLimit = 3;
    [SerializeField]
    private bool charmable = true;
    private int timesCharmed;
    private bool isSuperCharmed;
    private bool witchInShop;

    private void Start()
    {
        playerSpotter.playerSpotted += SetWitchInShop;
        playerSpotter.playerLeft += SetWitchLeft;
    }
    public void ApplyCharm()
    {
        if (charmable)
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
            charmable = false;
        }
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        animator.SetTrigger("Attacked");
    }

    private void SetWitchInShop()
    {
        witchInShop = true;
    }

    private void SetWitchLeft()
    {
        witchInShop = false;
    }

    private void CheckStatus()
    {
        if (!isSuperCharmed && witchInShop)
        {
            animator.SetTrigger("Angry");
        }
    }
    private void ResetCharmable()
    {
        charmable = true;
    }
}
