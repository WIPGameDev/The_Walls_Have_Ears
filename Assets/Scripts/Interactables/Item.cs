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

    public override void LoadSaveData(string json)
    {
        ItemSaveData data = JsonUtility.FromJson<ItemSaveData>(json);
        gameObject.SetActive(data.isActive);
    }

    public override string GetSaveData()
    {
        ItemSaveData data = new ItemSaveData();
        data.objectSceneID = this.ObjectSceneID;
        data.isActive = gameObject.activeInHierarchy;
        return JsonUtility.ToJson(data);
    }
}
