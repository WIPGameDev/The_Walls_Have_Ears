using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : Interactable
{
    public override void OnActivation()
    {
        gameController.ShowSaveAndLoadMenu();
    }
}
