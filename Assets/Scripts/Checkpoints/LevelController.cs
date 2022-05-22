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

    public SceneSaveData GetLevelSaveData ()
    {
        ISaveable[] saveables;
        SceneSaveData saveData = new SceneSaveData();

        Scene currentScene = SceneManager.GetSceneAt(1);
        saveData.scene = SceneManager.GetSceneAt(1);

        saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
        saveData.objects = new ObjectSaveData[saveables.Length];
        for (int i = 0; i < saveables.Length; i++)
        {
            saveData.objects[i] = saveables[i].GetSaveData();
        }
        return saveData;
    }

    public void LoadLevelSaveData (SceneSaveData saveData)
    {
        Dictionary<string,ISaveable> saveables;
        saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToDictionary(p => p.ObjectSceneID);

        foreach (ObjectSaveData objectSaveData in saveData.objects)
        {
            saveables[objectSaveData.objectSceneID].LoadSaveData(objectSaveData);
        }
    }
}
