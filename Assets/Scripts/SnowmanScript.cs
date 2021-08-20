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

    public int life;

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
        if (!snowpantsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Snowpants_Death"))
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
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
