using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Callbacks;

public class LevelSceneSetup : ScriptableObject
{
    [SerializeField] public SceneSetup[] scenes;

    [OnOpenAsset(1)]
    public static bool LoadSceneSetup(int instanceID, int line)
    {
        if (Selection.activeObject as LevelSceneSetup != null)
        {
            LevelSceneSetup levelSetup = (LevelSceneSetup)EditorUtility.InstanceIDToObject(instanceID);
            EditorSceneManager.RestoreSceneManagerSetup(levelSetup.scenes);
            return true;
        }
        return false;
    }
}
