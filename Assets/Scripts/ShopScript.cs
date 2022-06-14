using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ShopScript : InteractableScript
{
    [SerializeField]
    GameObject itemUIPrefab;
    [SerializeField]
    Transform itemContainer;
    [SerializeField]
    List<Item> items;
    private WitcherScript witcher;
    private bool shopInUse;

    new

        // Start is called before the first frame update
        void Start()
    {
        base.Start();
        foreach(Item item in items)
        {
            CreateItemButton(item);
        }
        HideShop();
    }
    public void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public override void Interact()
    {
        Debug.Log("Interact");
        if (shopInUse)
        {
            HideShop();
        }
        else
        {
            ShowShop(FindObjectOfType<WitcherScript>());
        }
    }

    private void CreateItemButton(Item item)
    {
        ItemUIScript itemUI = Instantiate(itemUIPrefab, itemContainer).GetComponent<ItemUIScript>();
        itemUI.setItemShop(item,this);
    }

    public void TryBuyItem(Item item)
    {
        if (witcher.TrySpendCandyAmount(item.candyCost))
        {
            witcher.BoughtItem(item);
        }
    }

    public void ShowShop(WitcherScript witcher)
    {
        if (witcher != null)
        {
            this.witcher = witcher;
            witcher.EnterShop();
            itemContainer.gameObject.SetActive(true);
            shopInUse = true;
        }
    }

    public void HideShop()
    {
        if (witcher != null)
        {
            witcher.ExitShop();
            this.witcher = null;
        }
        itemContainer.gameObject.SetActive(false);
        shopInUse = false;
    }
}
