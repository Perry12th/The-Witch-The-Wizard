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

    public void SpawnSnowball()
    {
        GrowingSnowballScript snowball = Instantiate(snowballPrefab, projPosition.position, projPosition.rotation).GetComponent<GrowingSnowballScript>();
        snowball.SetSpeed(startingSnowballSpeed);
    }

}
