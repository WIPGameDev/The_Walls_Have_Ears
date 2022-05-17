using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(LevelSceneSetup))]
public class LevelSetupInspector : Editor
{
    public override void OnInspectorGUI()
    {
        LevelSceneSetup levelSetup = (LevelSceneSetup)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Load Scene Setup"))
        {
            EditorSceneManager.RestoreSceneManagerSetup(levelSetup.scenes);
        }
    }
}