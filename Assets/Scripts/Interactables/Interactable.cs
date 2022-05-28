using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected UnityEvent interactionEvent;

    protected GameController gameController;

    protected virtual void Start()
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
}
