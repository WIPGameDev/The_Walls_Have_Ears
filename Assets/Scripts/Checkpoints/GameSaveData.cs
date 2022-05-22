using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveData : ISerializationCallbackReceiver
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public bool flashlightOn;
    public string savedScene;
    public Dictionary<string, SceneSaveData> sceneSaveData;
    public Dictionary<string, InventoryItem> items;

    [SerializeField] private List<string> sceneSaveDataKeys = new List<string>();
    [SerializeField] private List<SceneSaveData> sceneSaveDataValues = new List<SceneSaveData>();
    [SerializeField] private List<string> itemsKeys = new List<string>();
    [SerializeField] private List<InventoryItem> itemsValues = new List<InventoryItem>();

    public GameSaveData ()
    {
        sceneSaveData = new Dictionary<string, SceneSaveData>();
    }

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

    public SceneSaveData GetSceneData (Scene scene)
    {
        if (sceneSaveData.ContainsKey(scene.name))
        {
            return sceneSaveData[scene.name];
        }
        return null;
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        sceneSaveDataKeys.Clear();
        sceneSaveDataValues.Clear();
        itemsKeys.Clear();
        itemsValues.Clear();
        
        foreach (KeyValuePair<string, SceneSaveData> pair in sceneSaveData)
        {
            sceneSaveDataKeys.Add(pair.Key);
            sceneSaveDataValues.Add(pair.Value);
        }

        foreach (KeyValuePair<string, InventoryItem> pair in items)
        {
            itemsKeys.Add(pair.Key);
            itemsValues.Add(pair.Value);
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        sceneSaveData = new Dictionary<string, SceneSaveData>();
        items = new Dictionary<string, InventoryItem>();

        for (int i = 0; i != Mathf.Min(sceneSaveDataKeys.Count, sceneSaveDataValues.Count); i++)
        {
            sceneSaveData.Add(sceneSaveDataKeys[i], sceneSaveDataValues[i]);
        }

        for (int i = 0; i != Mathf.Min(itemsKeys.Count, itemsValues.Count); i++)
        {
            items.Add(itemsKeys[i], itemsValues[i]);
        }
    }
}
