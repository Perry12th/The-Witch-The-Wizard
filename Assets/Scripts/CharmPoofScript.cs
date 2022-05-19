using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class CharmPoofScript : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem charmPoofEffect;
    //[SerializeField]
    //private Vector3 startingVelocity;
    private List<ICharmable> charmables = new List<ICharmable>();
    private bool lookingRight = true;
    private bool charmApplied = false;
    private float timer;
    private float lifetimeMax;
    private float lifetimeMin;
    // Start is called before the first frame update
    void Start()
    {
        lifetimeMax = charmPoofEffect.main.duration;
        lifetimeMin = lifetimeMax / 2;
        charmPoofEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifetimeMax)
        {
            Destroy(gameObject);
        }
        else if (timer > lifetimeMin && !charmApplied)
        {
            PreformCharm();
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        ICharmable charmable = other.GetComponent<ICharmable>();
        if (charmable != null && !charmables.Contains(charmable))
        {
            charmables.Add(charmable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICharmable charmable = other.GetComponent<ICharmable>();
        if (charmable != null && charmables.Contains(charmable))
        {
            charmables.Remove(charmable);
        }
    }
    public void setFacing(bool lookingRight)
    {
        this.lookingRight = lookingRight; 
        if (this.lookingRight)
        {
            //setStartingVelocity(startingVelocity);
            transform.eulerAngles = new Vector3(0,0, 0);
        }
        else
        {
            //setStartingVelocity(-startingVelocity);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    //private void setStartingVelocity(Vector3 startingVelocity)
    //{
    //    charmPoofEffect.SetVector3("StartingVelocity", startingVelocity);
    //}

    private void PreformCharm()
    {
        foreach(ICharmable charmable in charmables)
        {
            charmable.ApplyCharm();
        }
        charmApplied = true;
    }

}
