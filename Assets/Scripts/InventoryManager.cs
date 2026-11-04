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

    public bool AddItem(InventoryItemData item)
    {
        if (item == null || string.IsNullOrEmpty(item.id))
        {
            return false;
        }

        if (items.Exists(existing => existing != null && existing.id == item.id))
        {
            return false;
        }

        items.Add(item);
        ItemAdded?.Invoke(item);
        return true;
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

        ItemRemoved?.Invoke(item);
        return true;
    }

    public IReadOnlyList<InventoryItemData> GetItems()
    {
        return items;
    }

    public bool TryUseItem(string itemId)
    {
        return HasItem(itemId);
    }

    public void ResetInventory()
    {
        items.Clear();
    }
}
