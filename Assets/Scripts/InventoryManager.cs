using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly List<InventoryItemData> items = new List<InventoryItemData>();

    public event Action<InventoryItemData> ItemAdded;
    public event Action<InventoryItemData> ItemRemoved;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(InventoryItemData item)
    {
        if (item == null)
        {
            return;
        }

        if (items.Exists(existing => existing != null && existing.id == item.id))
        {
            return;
        }

        items.Add(item);

        if (item.id == "staff_key")
        {
            GameManager.Instance?.SetFlag(GameFlagNames.HasStaffKey, true);
        }
        else
        {
            GameManager.Instance?.SetFlag(item.id, true);
        }

        ItemAdded?.Invoke(item);
    }

    public bool HasItem(string itemId)
    {
        return items.Exists(item => item != null && item.id == itemId);
    }

    public bool RemoveItem(string itemId)
    {
        var item = items.Find(existing => existing != null && existing.id == itemId);
        if (item == null)
        {
            return false;
        }

        items.Remove(item);

        if (item.id == "staff_key")
        {
            GameManager.Instance?.SetFlag(GameFlagNames.HasStaffKey, false);
        }
        else
        {
            GameManager.Instance?.SetFlag(item.id, false);
        }

        ItemRemoved?.Invoke(item);
        return true;
    }

    public IReadOnlyList<InventoryItemData> GetItems()
    {
        return items;
    }

    public bool TryUseItem(string itemId)
    {
        if (!HasItem(itemId))
        {
            return false;
        }

        var item = items.Find(existing => existing != null && existing.id == itemId);
        if (item == null)
        {
            return false;
        }

        if (item.id == "staff_key")
        {
            GameManager.Instance?.SetFlag(GameFlagNames.HasStaffKey, true);
            RemoveItem(itemId);
            return true;
        }

        return false;
    }
}
