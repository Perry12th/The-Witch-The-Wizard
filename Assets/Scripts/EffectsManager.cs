using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject smokePoof;
    [SerializeField]
    private GameObject lightingBlast;
    [SerializeField]
    private GameObject fireBlast;
    #region Singleton
    public static EffectsManager instance;

    private void Awake()
    {
        //Make sure there is only one instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public void SpawnSmokePoof(Transform transform)
    {
        Instantiate(smokePoof, transform.position, transform.rotation).transform.localScale = transform.localScale;
    }
    public void SpawnSmokePoof(Vector3 position, Quaternion quaternion, Vector3 localScale)
    {
        Instantiate(smokePoof, position, quaternion).transform.localScale = localScale;
    }

    public void SpawnLightingBlast(Transform transform)
    {
        Instantiate(lightingBlast, transform.position, transform.rotation).transform.localScale = transform.localScale;
    }

    public void SpawnLightingBlast(Vector3 position, Quaternion quaternion, Vector3 localScale)
    {
        Instantiate(lightingBlast, position, quaternion).transform.localScale = localScale;
    }

    public void SpawnFireBlast(Transform transform)
    {
        Instantiate(fireBlast, transform.position, transform.rotation).transform.localScale = transform.localScale;
    }

    public void SpawnFireBlast(Vector3 position, Quaternion quaternion, Vector3 localScale)
    {
        Instantiate(fireBlast, position, quaternion).transform.localScale = localScale;
    }

}
