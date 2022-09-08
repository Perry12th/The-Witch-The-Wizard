using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTreeScript : InteractableScript
{
    [SerializeField]
    private WitcherScript.SpellType spellType;
    [SerializeField]
    private Conversation interactedConversation;
    [SerializeField]
    private Renderer glowingRingRenderer;
    [SerializeField]
    private float fadeEffectTime = 2.0f;

    public void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    public override void Interact()
    {
        WitcherScript witch = FindObjectOfType<WitcherScript>();
        if (witch != null)
        {
            witch.EnableSpell(spellType);
            witch.SetDialogue(interactedConversation);
            isInteractable = false;
            interactionCollider.enabled = false;
            interactionText.enabled = false;
            if (glowingRingRenderer != null)
            {
                DisableParticleEffects();
                StartCoroutine(FadeAwayRingEffect());
            }
        }
    }

    private IEnumerator FadeAwayRingEffect()
    {
        Color materialColor = glowingRingRenderer.material.GetColor("Color_3728627B");
        float startingAlpha = materialColor.a;
        float timer = 0;

        while (timer < fadeEffectTime)
        {
            timer += Time.deltaTime;
            materialColor.a = Mathf.Lerp(startingAlpha, 0, timer / fadeEffectTime);
            glowingRingRenderer.material.SetColor("Color_3728627B", materialColor); 
            yield return new WaitForEndOfFrame();
        }
    }

    private void DisableParticleEffects()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in particleSystems)
        {
            ParticleSystem.MainModule main = particle.main;
            main.loop = false;
        }
    }

}
