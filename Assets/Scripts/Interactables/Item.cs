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
        gameObject.SetActive(false);
    }

    public override void LoadSaveData (ObjectSaveData objectSaveData)
    {
        gameObject.SetActive(objectSaveData.isActive);
    }

    public override ObjectSaveData GetSaveData()
    {
        ObjectSaveData data = new ObjectSaveData();
        data.objectSceneID = this.ObjectSceneID;
        data.isActive = gameObject.activeInHierarchy;
        return data;
    }
}
