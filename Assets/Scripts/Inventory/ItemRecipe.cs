using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 2)]
public class ItemRecipe : ScriptableObject
{
    [SerializeField] private InventoryItem[] componentItems;
    [SerializeField, HideInInspector] private bool[] collected;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private bool spawnRelativeToPlayer = true;
    [SerializeField] private GameObject prefab;

    public GameObject Prefab { get => prefab; }
    public InventoryItem[] ComponentItems { get => componentItems; }
    public Vector3 SpawnPosition { get => spawnPosition; }
    public bool SpawnRelativeToPlayer { get => spawnRelativeToPlayer; }

    void OnValidate()
    {
        ResetCollected();
    }

    public bool Contains (InventoryItem item)
    {
        if (componentItems != null)
        {
            foreach (InventoryItem component in componentItems)
            {
                if (item.ItemName == component.ItemName)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetCollected (InventoryItem item)
    {
        if (componentItems != null)
        {
            for (int i = 0; i < componentItems.Length; i++)
            {
                if (componentItems[i].ItemName == item.ItemName)
                {
                    collected[i] = true;
                }
            }
        }
    }

    public bool RecipeCompleted ()
    {
        foreach (bool isCollected in collected)
        {
            if (!isCollected)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetCollected ()
    {
        if (componentItems != null)
        {
            collected = new bool[componentItems.Length];
        }
    }
}
