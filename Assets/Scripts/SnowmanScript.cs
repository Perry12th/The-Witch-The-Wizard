using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanScript : MonoBehaviour
{
    [SerializeField]
    GameObject snowballPrefab;
    [SerializeField]
    Transform projPosition;
    [SerializeField]
    Animator snowpantsAnimator;
    [SerializeField]
    private float startingSnowballSpeed = 1f;
    [SerializeField]
    private GameObject snowballHolder;
    [SerializeField]
    private float maxSnowBallSize = 6.0f;
    [SerializeField]
    private float snowBallgrowSpeed = 0.01f;
    [SerializeField]
    private Color deathColor;
    [SerializeField]
    private Renderer snowPantsRenderer;
    [SerializeField]
    private float flashTimeRate = 0.2f;
    [SerializeField]
    private int life = 2;
    [SerializeField]
    private float attackTime = 5.0f;
    private float chargeTimer = 0.0f;
    private SnowpantsStates snowpantsState = SnowpantsStates.IDLE;

    private enum SnowpantsStates
    { 
        IDLE,
        ATTACKING,
        HURTING,
        DYING,
    }

    private void Update()
    {
        if (snowpantsState == SnowpantsStates.IDLE)
        {
            chargeTimer += Time.deltaTime;
            if (attackTime < chargeTimer)
            {
                ChargeUpSnowBall();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            Destroy(collision.gameObject);
            life--;

            if (life > 0)
            {
                snowpantsAnimator.SetTrigger("Hurt");
                snowballHolder.SetActive(false);
                snowpantsState = SnowpantsStates.HURTING;
            }
            if (life == 0)
            {
                snowpantsState = SnowpantsStates.DYING;
                snowpantsAnimator.SetTrigger("Death");
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (snowpantsState != SnowpantsStates.DYING)
                {
                    collision.gameObject.GetComponent<WitcherScript>().PlayerDeath();
                }
            }
        }
    }

    private void ChargeUpSnowBall()
    {
        if (snowpantsState != SnowpantsStates.HURTING || snowpantsState != SnowpantsStates.DYING)
        {
            snowpantsAnimator.SetTrigger("Attack2");
            snowpantsState = SnowpantsStates.ATTACKING;
        }
    }

    public void ShowSnowballHand()
    {
        snowballHolder.SetActive(true);
    }


    public void SpawnSnowball()
    {
        chargeTimer = 0;
        snowballHolder.SetActive(false);
        GrowingSnowballScript snowball = Instantiate(snowballPrefab, snowballHolder.transform.position, snowballPrefab.transform.rotation).GetComponent<GrowingSnowballScript>();
        snowball.SetSpeed(startingSnowballSpeed);
        snowball.SetMaxSize(maxSnowBallSize);
        snowball.setGrowthSpeed(snowBallgrowSpeed);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ReturnToIdle()
    {
        snowpantsState = SnowpantsStates.IDLE;
    }

    public void TurnToBlack()
    {
        StartCoroutine(TurningToBlack());
    }

    public void StartFlashing()
    {
        StartCoroutine(Flashing());
        snowballHolder.SetActive(false);
    }

    private IEnumerator Flashing()
    {
        float flashTime = snowpantsAnimator.GetCurrentAnimatorStateInfo(0).length;
        bool flashing = false;
        chargeTimer = 0;
        float flashTimer = 0;

        while (chargeTimer < flashTime)
        {
            chargeTimer += Time.deltaTime;
            flashTimer += Time.deltaTime;

            if (flashTimer >= flashTimeRate)
            {
                flashTimer = 0;
                flashing = !flashing;

                // Flashing = true means the renderers are disabled
                snowPantsRenderer.enabled = !flashing;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void StopFlashing()
    {
        StopAllCoroutines();
        snowPantsRenderer.enabled = true;
    }

    IEnumerator TurningToBlack()
    {
        snowballHolder.SetActive(false);
        Color snowPantsColor = snowPantsRenderer.material.color;
        float animTime = (snowpantsAnimator.GetCurrentAnimatorStateInfo(0).length * 0.2f);
        float timer = 0;

        Debug.Log("Turn To Black");
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            snowPantsRenderer.material.SetColor("_Color", Color.Lerp(snowPantsColor, deathColor, timer / animTime));
            yield return new WaitForEndOfFrame();
        }
    }
}
