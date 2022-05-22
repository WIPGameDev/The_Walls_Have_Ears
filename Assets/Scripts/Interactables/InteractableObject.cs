using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : Interactable, ISaveable
{
    [SerializeField] private string objectSceneID = Guid.NewGuid().ToString();

    public string ObjectSceneID { get => objectSceneID; }

    public abstract ObjectSaveData GetSaveData();

    public abstract void LoadSaveData(ObjectSaveData objectSaveData);
}