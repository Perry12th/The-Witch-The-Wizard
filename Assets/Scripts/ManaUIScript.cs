using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaUIScript : MonoBehaviour
{
    [SerializeField]
    private WitcherScript witcher;
    [SerializeField]
    private RawImage manaBar;
    [SerializeField]
    private RectTransform barMaskRectTransform;
    [SerializeField]
    private float uvXSpeed = 0.1f;
    private float barMaskWidth;

    private void Awake()
    {
        barMaskWidth = barMaskRectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        //manaBar.fillAmount = (witcher.GetManaAmount() / witcher.GetMaxMana());

        Rect uvRect = manaBar.uvRect;
        uvRect.x -= uvXSpeed * Time.deltaTime;
        manaBar.uvRect = uvRect;

        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = (witcher.GetManaAmount() / witcher.GetMaxMana()) * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;
    }
}
