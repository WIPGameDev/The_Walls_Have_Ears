using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionInteractable : InteractableObject
{
    [SerializeField] private string transitionSceneTarget;
    [SerializeField] private string transitionMarkerTarget;

    public override void Activate()
    {
        base.Activate();
        gameController.LoadLevel(transitionSceneTarget, transitionMarkerTarget);
    }
}