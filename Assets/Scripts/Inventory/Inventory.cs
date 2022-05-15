using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Dictionary<string, InventoryItem> items;

    private void Awake()
    {
        items = new Dictionary<string, InventoryItem>();
    }

    public void AddItem (InventoryItem newItem)
    {
        items.TryAdd(newItem.ItemName, newItem);
    }

    public void RemoveItem (InventoryItem removeItem)
    {
        items.Remove(removeItem.ItemName);
    }

    public bool FindItem (string key, out InventoryItem invItem)
    {
        bool found = false;
        found = items.TryGetValue(key, out invItem);
        return found;
    }
}
