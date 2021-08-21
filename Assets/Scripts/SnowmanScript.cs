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
    private float timer = 0;
    [SerializeField]
    private int life;

    private void Start()
    {
        InvokeRepeating("ChargeUpSnowBall", 0, 10);
        life = 2;
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
            }
            if (life == 0)
            {
                snowpantsAnimator.SetTrigger("Death");
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (life > 0)
                {
                    collision.gameObject.GetComponent<WitcherScript>().PlayerDeath();
                }
            }
        }
    }

    private void ChargeUpSnowBall()
    {
        if (!snowpantsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Snowpants_Death") || life > 0)
        snowpantsAnimator.SetTrigger("Attack2");
    }

    public void ShowSnowballHand()
    {
        snowballHolder.SetActive(true);
    }


    public void SpawnSnowball()
    {
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

    public void TurnToBlack()
    {
        StartCoroutine(TurningToBlack());
        snowballHolder.SetActive(false);
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
        timer = 0;
        float flashTimer = 0;

        while (timer < flashTime)
        {
            timer += Time.deltaTime;
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
        Color snowPantsColor = snowPantsRenderer.material.color;
        float animTime = (snowpantsAnimator.GetCurrentAnimatorStateInfo(0).length * 0.2f);
        timer = 0;

        Debug.Log("Turn To Black");
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            snowPantsRenderer.material.SetColor("_Color", Color.Lerp(snowPantsColor, deathColor, timer / animTime));
            yield return new WaitForEndOfFrame();
        }
    }
}
