using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaveData
{
    public string objectSceneID;
}

public class TriggerSaveData : ObjectSaveData
{
    public bool isActive;
}

public class ItemSaveData : ObjectSaveData
{
    public bool isActive;
}

public class LockableSaveData : ObjectSaveData
{
    public bool locked;
}

public class DoorSaveData : LockableSaveData
{
    public bool opened;
}
