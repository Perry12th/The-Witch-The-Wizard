using PathCreation.Examples;
using System.Collections;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private WitcherScript witcher;
    [SerializeField] private Animator cartAnimator;
    [SerializeField] private Light rightEyeLight;
    [SerializeField] private Light leftEyeLight;
    [SerializeField] private ParticleSystem sparksParticle;
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private GameObject interactionText;
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private Collider rollercoasterCollider;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Collider hiddenCollider;
    [SerializeField] private Collider cartCollider;
    [SerializeField] private Transform cartSeat;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private Vector3 dropRotation;
    [SerializeField] private Vector3 blastForce;
    [SerializeField] bool powered;

    public void Start()
    {
        SetUpCart();
    }

    private void SetUpCart()
    {
        rightEyeLight.enabled = false;
        leftEyeLight.enabled = false;
        triggerCollider.enabled = false;
        interactionText.SetActive(false);
    }
    
    

    public void BoardCart()
    {
        if (!witcher.GetIsLookingRight())
        {
            witcher.FlipRight();
        }
        witcher.GetRigidBody().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        witcher.transform.parent = cartSeat;
        witcher.transform.position = new Vector3(cartSeat.position.x, cartSeat.position.y, cartSeat.position.z);
        witcher.AllowMovement(false);
        sparksParticle.Play();
        pathFollower.enabled = true;
        interactionCollider.enabled = false;
        rollercoasterCollider.enabled = false;
        hiddenCollider.enabled = false;
        cartCollider.enabled = false;
        interactionText.SetActive(false);
        rigidbody.constraints = RigidbodyConstraints.None;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && powered)
        {
            BoardCart();
        }
    }

    public void AdjustSpeed(float newSpeed, float time)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSpeed(newSpeed, time));
    }

    private IEnumerator ChangeSpeed(float newSpeed, float time)
    {
        float startSpeed = pathFollower.GetSpeed();
        float timer = 0;

        while (timer < time)
        {
            timer += Time.deltaTime;
            pathFollower.SetSpeed(Mathf.Lerp(startSpeed, newSpeed, time / timer));
            yield return new WaitForEndOfFrame();
        }
        pathFollower.SetSpeed(newSpeed);
    }

    public void LeaveCart()
    {
        DisableEffects();
        sparksParticle.Stop();
        fireParticle.Play();
        pathFollower.enabled = false;
        EffectsManager.instance.SpawnSmokePoof(cartSeat);
        witcher.GetRigidBody().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        witcher.transform.parent = null;
        witcher.transform.position = dropPoint.position;
        witcher.transform.rotation = Quaternion.Euler(dropRotation);
        witcher.AllowMovement(true);
        EffectsManager.instance.SpawnFireBlast(transform);
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.useGravity = true;
        rigidbody.AddForce(blastForce, ForceMode.Impulse);
        EffectsManager.instance.SpawnSmokePoof(witcher.transform);
    }

    public void EnableEffects()
    {
        cartAnimator.SetTrigger("Powered");
        rightEyeLight.enabled = true;
        leftEyeLight.enabled = true;
        triggerCollider.enabled = true;
        interactionText.SetActive(true);
        powered = true;
    }

    public void DisableEffects()
    {
        cartAnimator.SetTrigger("DePowered");
        rightEyeLight.enabled = false;
        leftEyeLight.enabled = false;
        powered = false;
    }

}
