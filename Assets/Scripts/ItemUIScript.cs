using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;
    [SerializeField] Image itemIconImage;
    [SerializeField] TextMeshProUGUI itemCostText;
    [SerializeField] Button button;
    [SerializeField] ShopScript shop;
    private Item item;
    
    public void setItemShop(Item item, ShopScript shop)
    {
        this.shop = shop;
        this.item = item;
        itemNameText.text = item.name;
        //itemDescriptionText.text = item.description;
        itemIconImage.sprite = item.icon;
        itemCostText.text = item.candyCost.ToString();
    }

    public void TryBuyItem()
    {
        if (item != null)
        {
            shop.TryBuyItem(item);
        }
    }
}
