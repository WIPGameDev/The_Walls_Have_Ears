using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] protected UnityEvent interactionEvent;

    public virtual void Activate()
    {
        if (interactionEvent != null)
            interactionEvent.Invoke();
    }
}