using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDInventoryView
{
    private readonly Action<string, float> showFeedback;
    private readonly List<VisualElement> slots = new();
    private VisualElement inventoryPanel;

    public HUDInventoryView(MonoBehaviour owner, VisualElement root, Action<string, float> showFeedback)
    {
        this.showFeedback = showFeedback;
        inventoryPanel = root.Q<VisualElement>("inventory-panel");
        slots.Clear();
        slots.Add(root.Q<VisualElement>("slot-0"));
        slots.Add(root.Q<VisualElement>("slot-1"));
        slots.Add(root.Q<VisualElement>("slot-2"));
    }

    public void Bind()
    {
        foreach (var slot in slots)
        {
            if (slot == null)
            {
                continue;
            }
            slot.UnregisterCallback<ClickEvent>(OnSlotClicked);
            slot.RegisterCallback<ClickEvent>(OnSlotClicked);
            slot.UnregisterCallback<ContextClickEvent>(OnSlotContextClicked);
            slot.RegisterCallback<ContextClickEvent>(OnSlotContextClicked);
            slot.UnregisterCallback<MouseDownEvent>(OnSlotMouseDown);
            slot.RegisterCallback<MouseDownEvent>(OnSlotMouseDown);
        }
    }

    public void Refresh()
    {
        IReadOnlyList<InventoryItemData> items = Array.Empty<InventoryItemData>();
        if (InventoryManager.Instance != null)
        {
            items = InventoryManager.Instance.GetItems();
        }

        for (var index = 0; index < slots.Count; index++)
        {
            var slot = slots[index];
            if (slot == null)
            {
                continue;
            }

            var item = index < items.Count ? items[index] : null;
            slot.userData = item != null ? item.id : null;
            slot.EnableInClassList("full", item != null);
            slot.EnableInClassList("empty", item == null);

            var label = slot.Q<Label>("slot-label");
            if (label == null)
            {
                label = new Label { name = "slot-label" };
                label.AddToClassList("slot-label");
                slot.Add(label);
            }

            var selected = item != null
                && InventoryManager.Instance != null
                && item.id == InventoryManager.Instance.SelectedItemId;
            slot.EnableInClassList("selected", selected);
            label.text = item == null ? string.Empty : GetSlotLabel(item);
            label.style.display = item == null ? DisplayStyle.None : DisplayStyle.Flex;
            slot.style.marginRight = index == slots.Count - 1 ? 0 : 12;
            slot.pickingMode = item == null ? PickingMode.Ignore : PickingMode.Position;
            slot.focusable = item != null;
        }
    }

    public void RefreshLantern(
        VisualElement lanternIcon,
        InventoryManager inventory,
        GameManager gameManager,
        string itemId,
        Sprite notObtained,
        Sprite obtained)
    {
        if (lanternIcon == null)
        {
            return;
        }

        var hasLantern = inventory != null && inventory.HasItem(itemId);
        if (!hasLantern && gameManager != null)
        {
            hasLantern = gameManager.HasFlag(GameFlagNames.HasFlashlight);
        }

        var sprite = hasLantern && obtained != null ? obtained : notObtained;
        lanternIcon.style.backgroundImage = sprite != null
            ? new StyleBackground(sprite)
            : new StyleBackground();
        lanternIcon.style.opacity = hasLantern ? 1f : 0.9f;
    }

    public void SetVisible(bool visible)
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    private void OnSlotClicked(ClickEvent evt)
    {
        var slot = ResolveSlot(evt.target as VisualElement);
        var itemId = slot != null ? slot.userData as string : null;
        if (string.IsNullOrEmpty(itemId) || InventoryManager.Instance == null)
        {
            return;
        }

        var item = InventoryManager.Instance.GetItemById(itemId);
        if (item == null)
        {
            return;
        }

        InventoryManager.Instance.SelectItem(itemId);
        showFeedback($"{item.displayName} selected.", 1.1f);
        Refresh();
    }

    private void OnSlotContextClicked(ContextClickEvent evt)
    {
        InspectSlot(evt.target as VisualElement, evt);
    }

    private void OnSlotMouseDown(MouseDownEvent evt)
    {
        if (evt.button == (int)MouseButton.RightMouse)
        {
            InspectSlot(evt.target as VisualElement, evt);
        }
    }

    private void InspectSlot(VisualElement target, EventBase evt)
    {
        var slot = ResolveSlot(target);
        var itemId = slot != null ? slot.userData as string : null;
        if (string.IsNullOrEmpty(itemId) || InventoryManager.Instance == null)
        {
            return;
        }

        var item = InventoryManager.Instance.GetItemById(itemId);
        if (item == null)
        {
            return;
        }

        var reality = RealityManager.Instance != null
            ? RealityManager.Instance.CurrentReality
            : RealityType.Normal;
        var description = item.GetDescriptionForReality(reality);

        if (item.id.Equals("worn_book", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null
            && !GameManager.Instance.HasFlag(GameFlagNames.FloppyDiskRecovered))
        {
            var disk = new InventoryItemData
            {
                id = "floppy_disk",
                displayName = "Floppy Disk",
                description = "A small disk hidden inside the book.",
                descriptionInNormalReality = "A small, faded floppy disk has been tucked inside the back pages.",
                descriptionInOtherReality = "The disk looks like it was placed there to be found later.",
                isKeyItem = true
            };

            if (!InventoryManager.Instance.HasItem(disk.id))
            {
                InventoryManager.Instance.AddItem(disk);
                GameManager.Instance.SetFlag(GameFlagNames.FloppyDiskRecovered, true);
                description = "You inspect the Worn Book and discover a hidden floppy disk.";
            }
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            description = $"You inspect {item.displayName}.";
        }

        showFeedback(description, 2f);
        Refresh();
        evt.StopPropagation();
    }

    private static VisualElement ResolveSlot(VisualElement target)
    {
        var slot = target;
        while (slot != null && !slot.ClassListContains("inventory-slot"))
        {
            slot = slot.parent;
        }
        return slot;
    }

    private static string GetSlotLabel(InventoryItemData item)
    {
        if (item == null)
        {
            return string.Empty;
        }
        if (!string.IsNullOrWhiteSpace(item.displayName))
        {
            var displayName = item.displayName.Trim();
            if (displayName.Contains("Key", StringComparison.OrdinalIgnoreCase))
            {
                return "K";
            }
            return displayName.Substring(0, 1).ToUpperInvariant();
        }
        return item.id != null && item.id.Length > 0
            ? item.id.Substring(0, 1).ToUpperInvariant()
            : "I";
    }
}
