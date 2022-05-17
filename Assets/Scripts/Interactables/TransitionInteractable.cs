using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionInteractable : LockableInteractable
{
    [SerializeField] private string transitionSceneTarget;
    [SerializeField] private string transitionMarkerTarget;

    public override void OnActivation()
    {
        gameController.LoadLevel(transitionSceneTarget, transitionMarkerTarget);
    }
}