using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] public string itemName;
    [SerializeField] public string description;
    [SerializeField] public Sprite icon;
    [SerializeField] public int candyCost;
}
