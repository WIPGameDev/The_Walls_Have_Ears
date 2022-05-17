using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject
{
    [SerializeField] private InventoryItem inventoryItem;

    public override void Activate()
    {
        base.Activate();
    }

    public override void OnActivation()
    {
        GameObject.FindObjectOfType<Inventory>().AddItem(inventoryItem);
        Destroy(gameObject);
    }
}
