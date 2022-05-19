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
    [SerializeField]
    private RectTransform lostBarMaskTransform;
    [SerializeField]
    private float lostBarSpeed = 1.0f;
    [SerializeField]
    private float lostBarWaitTime = 3.0f;
    private Vector2 barMaskSizeDelta;
    private Vector2 lostBarSizeDelta;
    private Rect uvRect;
    private float barMaskWidth;

    private void Awake()
    {
        barMaskWidth = barMaskRectTransform.sizeDelta.x;
        barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        lostBarSizeDelta = lostBarMaskTransform.sizeDelta;
        uvRect = manaBar.uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        //manaBar.fillAmount = (witcher.GetManaAmount() / witcher.GetMaxMana());
        uvRect.x -= uvXSpeed * Time.deltaTime;
        manaBar.uvRect = uvRect;

        barMaskSizeDelta.x = (witcher.GetManaAmount() / witcher.GetMaxMana()) * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;
    }

    public void InitLostManaBar()
    {
        if (barMaskSizeDelta.x >= lostBarSizeDelta.x)
        {
            lostBarSizeDelta = barMaskSizeDelta;
            lostBarMaskTransform.sizeDelta = lostBarSizeDelta;
        }
        StopAllCoroutines();
        StartCoroutine(AnimateLostManaBar());
    }

    private IEnumerator AnimateLostManaBar()
    {
        yield return new WaitForSeconds(lostBarWaitTime);

        while (barMaskSizeDelta.x <= lostBarSizeDelta.x)
        {
            lostBarSizeDelta.x -= lostBarSpeed * Time.deltaTime;
            lostBarMaskTransform.sizeDelta = lostBarSizeDelta;
            yield return new WaitForEndOfFrame();
        }
    }
}
