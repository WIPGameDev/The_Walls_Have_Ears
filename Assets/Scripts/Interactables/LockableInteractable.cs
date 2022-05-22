using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LockableInteractable : InteractableObject
{
    [SerializeField] protected bool locked = false;
    [SerializeField] protected InventoryItem keyItem;

    [SerializeField] protected UnityEvent blockedEvent;
    [SerializeField] protected UnityEvent lockedEvent;
    [SerializeField] protected UnityEvent unlockedEvent;

    public InventoryItem KeyItem { get => keyItem; set => keyItem = value; }

    public override void Activate()
    {
        if (locked)
        {
            if (CheckKey())
            {
                Unlock();
            }
            else
            {
                blockedEvent.Invoke();
            }
        }
        else
        {
            base.Activate();
        }
    }

    public bool CheckKey ()
    {
        if (keyItem != null)
        {
            if (GameObject.FindObjectOfType<Inventory>().UseItem(keyItem.ItemName))
            {
                return true;
            }
        }
        return false;
    }

    public void Lock()
    {
        keyItem = null;
        lockedEvent.Invoke();
        locked = true;
    }

    public void Lock (InventoryItem newKeyItem)
    {
        keyItem = newKeyItem;
        lockedEvent.Invoke();
        locked = true;
    }

    public void Unlock ()
    {
        unlockedEvent.Invoke();
        locked = false;
    }

    public override void LoadSaveData (ObjectSaveData objectSaveData)
    {
        this.locked = objectSaveData.locked;
    }

    public override ObjectSaveData GetSaveData()
    {
        ObjectSaveData objectSaveData = new ObjectSaveData();
        objectSaveData.objectSceneID = this.ObjectSceneID;
        objectSaveData.locked = this.locked;
        return objectSaveData;
    }
}
