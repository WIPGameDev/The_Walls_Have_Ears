using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Dictionary<string, InventoryItem> items;
    [SerializeField] private ItemRecipe bombRecipe;

    [SerializeField] private UnityEvent itemCollected;
    [SerializeField] private UnityEvent itemRemoved;
    [SerializeField] private UnityEvent itemUsed;
    [SerializeField] private UnityEvent craftingBomb;

    private Coroutine craftingCoroutine;

    public Dictionary<string, InventoryItem> Items { get => items; set => items = value; }

    private void Awake()
    {
        items = new Dictionary<string, InventoryItem>();
    }

    public void AddItem (InventoryItem newItem)
    {
        if (newItem != null)
        {
            if (items.TryAdd(newItem.ItemName, newItem))
            {
                itemCollected.Invoke();
                if (bombRecipe != null && bombRecipe.Contains(newItem))
                {
                    bombRecipe.SetCollected(newItem);
                    if (bombRecipe.RecipeCompleted())
                    {
                        craftingBomb.Invoke();
                        craftingCoroutine = StartCoroutine(Crafting());
                    }
                }
            }
        }
    }

    public void RemoveItem (InventoryItem removeItem)
    {
        if (removeItem != null)
        {
            itemRemoved.Invoke();
            items.Remove(removeItem.ItemName);
        }
    }

    public bool FindItem (string key, out InventoryItem invItem)
    {
        bool found = false;
        found = items.TryGetValue(key, out invItem);
        return found;
    }

    public bool ContainsItem (string key)
    {
        return items.ContainsKey(key);
    }

    public bool ContainsItem(InventoryItem value)
    {
        return items.ContainsValue(value);
    }

    public bool UseItem (string key)
    {
        InventoryItem usedItem;
        if (FindItem(key, out usedItem))
        {
            if (usedItem.LimitedUses)
            {
                usedItem.Uses -= 1;
                if (usedItem.Uses == 0)
                {
                    RemoveItem(usedItem);
                }
            }
            itemUsed.Invoke();
            return true;
        }
        return false;
    }


    private IEnumerator Crafting ()
    {
        Transform playerTransform = FindObjectOfType<PlayerController>().transform;
        if (playerTransform != null)
        {
            gameController.GameState = GameState.FADING;
            Time.timeScale = 0f;
            foreach (InventoryItem item in bombRecipe.ComponentItems)
            {
                bombRecipe.ResetCollected();
                RemoveItem(item);
            }
            Coroutine fading = StartCoroutine(gameController.Fading(1));
            yield return fading;
            Vector3 spawnPos = bombRecipe.SpawnRelativeToPlayer ? playerTransform.TransformPoint(bombRecipe.SpawnPosition) : bombRecipe.SpawnPosition;
            Instantiate(bombRecipe.Prefab, spawnPos, Quaternion.identity);
            yield return new WaitForSecondsRealtime(0.2f);
            fading = StartCoroutine(gameController.Fading(0));
            yield return fading;
            Time.timeScale = 1f;
            gameController.GameState = GameState.PLAYING;
        }
        else
        {
            Debug.LogError("No gameobject with playercontroller found.");
        }
    }

    /*
    */
    private void OnGUI()
    {
        if (items != null)
        {
            GUI.Box(new Rect(10, 10, 100, 10 + 50 * items.Count), "Inventory");
            int i = 1;
            foreach (KeyValuePair<string, InventoryItem> item in items)
            {
                GUI.Label(new Rect(20, 40 * i, 80, 50), item.Key);
                i++;
            }
        }
    }
}
