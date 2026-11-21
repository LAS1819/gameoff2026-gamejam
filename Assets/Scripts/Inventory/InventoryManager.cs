using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly List<InventoryItemData> items = new List<InventoryItemData>();

    public event Action<InventoryItemData> ItemAdded;
    public event Action<InventoryItemData> ItemRemoved;
    public event Action<string> SelectedItemChanged;

    public string SelectedItemId { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
        if (ItemAdded != null)
        {
            ItemAdded.Invoke(item);
        }
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

        if (SelectedItemId == item.id)
        {
            SelectedItemId = null;
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged.Invoke(null);
            }
        }

        if (ItemRemoved != null)
        {
            ItemRemoved.Invoke(item);
        }
        return true;
    }

    public IReadOnlyList<InventoryItemData> GetItems()
    {
        return items;
    }

    public InventoryItemData GetItemById(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            return null;
        }

        return items.Find(item => item != null && item.id == itemId);
    }

    public bool SelectItem(string itemId)
    {
        if (!HasItem(itemId))
        {
            return false;
        }

        if (SelectedItemId == itemId)
        {
            return true;
        }

        SelectedItemId = itemId;
        if (SelectedItemChanged != null)
        {
            SelectedItemChanged.Invoke(itemId);
        }
        return true;
    }

    public void ClearSelection()
    {
        DeselectSelectedItem();
    }

    public void DeselectSelectedItem()
    {
        if (string.IsNullOrEmpty(SelectedItemId))
        {
            return;
        }

        SelectedItemId = null;
        if (SelectedItemChanged != null)
        {
            SelectedItemChanged.Invoke(null);
        }
    }

    public bool TryUseItem(string itemId)
    {
        return HasItem(itemId);
    }

    public void ResetInventory()
    {
        items.Clear();
        ClearSelection();
    }
}
