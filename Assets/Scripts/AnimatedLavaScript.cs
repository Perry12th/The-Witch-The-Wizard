using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedLavaScript : MonoBehaviour
{
    [SerializeField]
    private float speedX = 0.1f;
    [SerializeField]
    private float speedY = 0.1f;

    private Renderer lavaRenderer;
    private Vector2 materialOffest;
    

    // Start is called before the first frame update
    void Start()
    {
        lavaRenderer = GetComponent<Renderer>();
        materialOffest = new Vector2(lavaRenderer.material.mainTextureOffset.x, lavaRenderer.material.mainTextureOffset.y);
    }

    // Update is called once per frame
    void Update()
    {
        materialOffest += new Vector2(Time.deltaTime * speedX, Time.deltaTime * speedY);
        lavaRenderer.material.SetTextureOffset("_BaseMap", materialOffest);
    }
}
