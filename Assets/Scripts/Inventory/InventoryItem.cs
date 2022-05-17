using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private int uses = 1;
    [SerializeField] private bool limitedUses = false;

    public string ItemName { get => itemName; }
    public int Uses { get => uses; set => uses = value; }
    public bool LimitedUses { get => limitedUses; set => limitedUses = value; }
}
