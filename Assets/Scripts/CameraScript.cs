using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform startingTarget;
    [SerializeField]
    private float zoomDuration;
    [SerializeField]
    private Vector3 startingOffset;
    [SerializeField] private BoxCollider boundsCollider;
    private Transform currentTarget;
    private Vector3 currentOffset;
    private bool trackingTarget = true;
    void Start()
    {
        //startingOffset = gameObject.transform.position - startingTarget.position;
        currentOffset = startingOffset;
        currentTarget = startingTarget;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (boundsCollider)
        {
            Vector3 delta = Vector3.zero;
            float dx = currentTarget.position.x - transform.position.x;
            // X axis
            if (dx > boundsCollider.bounds.extents.x || dx < -boundsCollider.bounds.extents.x)
            {
                if (transform.position.x < currentTarget.position.x)
                {
                    delta.x = dx - boundsCollider.bounds.extents.x;
                }
                else
                {
                    delta.x = dx + boundsCollider.bounds.extents.x;
                }
            }

            float dy = currentTarget.position.y - transform.position.y;
            // Y Axis
            if (dy > boundsCollider.bounds.extents.y || dy < -boundsCollider.bounds.extents.y)
            {
                if (transform.position.y < transform.position.y)
                {
                    delta.y = dy - boundsCollider.bounds.extents.y;
                }
                else
                {
                    delta.y = dy + boundsCollider.bounds.extents.y;
                }

            }
            Debug.Log("Dx:" +dx + "Dy:"+dy + "Delta:" + delta + "BoundsCollider:"+boundsCollider.bounds.extents);
            // Move the camera
            transform.position += delta;
        }

        if (trackingTarget)
        {
            gameObject.transform.position = currentTarget.position + currentOffset;
        }    
    }

    public void ChangeCameraPositionOffset(Transform newTarget, Vector3 newOffset)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeOffset(newOffset, zoomDuration));
        StartCoroutine(ChangeTarget(newTarget, zoomDuration));
    }

    public void ChangeCameraOffset(Vector3 newOffset, bool trackTarget = true)
    {
        StopAllCoroutines();
        trackingTarget = trackTarget;
        StartCoroutine(ChangeOffset(newOffset, zoomDuration));
    }

    public void ResetCamera(bool instant = true)
    {
        if (instant)
        {
            currentTarget = startingTarget;
            currentOffset = startingOffset;
        }

        if (currentTarget != startingTarget && currentOffset != startingOffset)
        {
            ChangeCameraPositionOffset(startingTarget, startingOffset);
        }
        else
        {
            ChangeCameraOffset(startingOffset);
        }
    }

    IEnumerator ChangeOffset(Vector3 newOffset, float timeDuration)
    {
        float timer = 0;
        Vector3 startingOffset = currentOffset;
        Vector3 resultingOffset = currentOffset;

        while (timer < timeDuration)
        {
            timer += Time.deltaTime;
            resultingOffset = Vector3.Lerp(startingOffset, newOffset, timer / timeDuration);
            currentOffset = resultingOffset;
            yield return new WaitForEndOfFrame();
        }
        
    }

    IEnumerator ChangeTarget(Transform newTarget, float timeDuration)
    {
        trackingTarget = false;
        float timer = 0;
        Vector3 startingPosition = transform.position;
        Vector3 resultingPosition = transform.position;

        while (timer < timeDuration)
        {
            timer += Time.deltaTime;
            resultingPosition = Vector3.Lerp(startingPosition, newTarget.position + currentOffset, timer / timeDuration);
            transform.position = resultingPosition;
            yield return new WaitForEndOfFrame();
        }

        currentTarget = newTarget;
        trackingTarget = true;
    }

    private void OnValidate()
    {
        if (startingTarget != null)
        {
            transform.position = startingTarget.position + startingOffset;
        }
    }
}
