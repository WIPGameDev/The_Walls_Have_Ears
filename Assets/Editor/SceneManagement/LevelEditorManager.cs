using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class LevelEditorManager : EditorWindow
{
    private string setupName;

    [MenuItem("Window/Level Editor Manager")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        LevelEditorManager window = (LevelEditorManager)EditorWindow.GetWindow(typeof(LevelEditorManager));
        window.Show();

    }

    void Awake()
    {
        setupName = "TestSetup";
    }

    void OnGUI()
    {
        GUILayout.Label("Save Scene Setup data", EditorStyles.boldLabel);

        setupName = GUILayout.TextField(setupName, 30);

        if (GUILayout.Button("Save Scene Setup"))
        {
            if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID("Assets/Editor/Levels/" + setupName + ".asset")))
            {
                Debug.Log("Creating Asset.");
                SaveSceneSetup();
            } else
            {
                Debug.Log("Scene Setup Assets with that name already exists.");
            }
        }
    }

    private void SaveSceneSetup()
    {
        LevelSceneSetup scenesetup = ScriptableObject.CreateInstance<LevelSceneSetup>();
        scenesetup.scenes = EditorSceneManager.GetSceneManagerSetup();

        AssetDatabase.CreateAsset(scenesetup, "Assets/Editor/Levels/" + setupName + ".asset");
        AssetDatabase.SaveAssets();
    }
}
