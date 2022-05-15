using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] protected UnityEvent interactionEvent;

    protected GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public virtual void Activate()
    {
        if (interactionEvent != null)
            interactionEvent.Invoke();
    }
}