using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractableObject), true)]
public class InteractableObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        InteractableObject interactableObject = (InteractableObject)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Regenarate ID"))
        {
            interactableObject.RegenerateID();
        }
    }
}
