using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveData
{
    public Dictionary<string, SceneSaveData> sceneSaveData;

    public bool ContainsScene (Scene scene)
    {
        return sceneSaveData.ContainsKey(scene.name);
    }

    public void StoreSceneData (SceneSaveData newSceneSaveData)
    {
        if (sceneSaveData.ContainsKey(newSceneSaveData.scene.name))
        {
            sceneSaveData[newSceneSaveData.scene.name] = newSceneSaveData;
        }
        else
        {
            sceneSaveData.TryAdd(newSceneSaveData.scene.name, newSceneSaveData);
        }
    }

    public SceneSaveData RetrieveSceneData (Scene scene)
    {
        if (sceneSaveData.ContainsKey(scene.name))
        {
            return sceneSaveData[scene.name];
        }
        return null;
    }
}
