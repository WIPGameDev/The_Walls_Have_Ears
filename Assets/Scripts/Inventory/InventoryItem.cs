using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private string itemName;

    public string ItemName { get => itemName; }
}
