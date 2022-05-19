using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
    string save;

    public string GetLevelSaveData ()
    {
        ISaveable[] saveables;
        SceneSaveData saveData = new SceneSaveData();

        Scene currentScene = SceneManager.GetSceneAt(1);
        saveData.scene = SceneManager.GetSceneAt(1);

        saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
        saveData.objects = new string[saveables.Length];
        for (int i = 0; i < saveables.Length; i++)
        {
            saveData.objects[i] = saveables[i].GetSaveData();
        }
        return JsonUtility.ToJson(saveData);
    }

    public void LoadLevelSaveData (string save)
    {
        Dictionary<string,ISaveable> saveables;
        SceneSaveData saveData = JsonUtility.FromJson<SceneSaveData>(save);
        saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToDictionary(p => p.GetObjectID());

        foreach (string data in saveData.objects)
        {
            ObjectSaveData objectSaveData = JsonUtility.FromJson<ObjectSaveData>(data);
            saveables[objectSaveData.objectSceneID].LoadSaveData(data);
        }
    }
}
