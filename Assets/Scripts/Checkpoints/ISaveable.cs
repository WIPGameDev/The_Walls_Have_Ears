using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string ObjectSceneID { get; }

    string GetSaveData();

    void LoadSaveData(string json);

    string GetObjectID();
}
