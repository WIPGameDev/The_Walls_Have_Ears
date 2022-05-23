using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string ObjectSceneID { get; }

    ObjectSaveData GetSaveData();

    void LoadSaveData(ObjectSaveData objectSaveData);
}
