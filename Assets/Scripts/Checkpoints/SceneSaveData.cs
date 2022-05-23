using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneSaveData
{
    public Scene scene;
    public ObjectSaveData[] objects;
}
