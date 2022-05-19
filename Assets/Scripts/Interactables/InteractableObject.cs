using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : MonoBehaviour, ISaveable
{
    [SerializeField] [HideInInspector] private string objectSceneID = Guid.NewGuid().ToString();
    [SerializeField] protected UnityEvent interactionEvent;

    protected GameController gameController;

    public string ObjectSceneID { get => objectSceneID; }

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public virtual void Activate()
    {
        if (interactionEvent != null)
            interactionEvent.Invoke();
        OnActivation();
    }

    public abstract void OnActivation();

    public virtual string GetObjectID()
    {
        return objectSceneID;
    }

    public abstract string GetSaveData();

    public abstract void LoadSaveData(string json);
}