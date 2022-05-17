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

    public bool ContainsItem (string key)
    {
        return items.ContainsKey(key);
    }

    public bool ContainsItem(InventoryItem value)
    {
        return items.ContainsValue(value);
    }

    public bool UseItem (string key)
    {
        InventoryItem usedItem;
        if (FindItem(key, out usedItem))
        {
            if (!usedItem.LimitedUses)
            {
                usedItem.Uses -= 1;
                if (usedItem.Uses == 0)
                {
                    RemoveItem(usedItem);
                }
            }
            return true;
        }
        return false;
    }
}
