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

            if (life <= 0)
            {
                Destroy(gameObject, 1.0f);
                snowpantsAnimator.SetTrigger("Hurt");
            }
        }
    }

    private void ChargeUpSnowBall()
    {
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
    }

}
