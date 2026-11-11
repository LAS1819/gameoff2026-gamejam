using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class HUDController : MonoBehaviour
{
    [Header("Runtime References")]
    [SerializeField] private PanelRenderer panelRenderer;

    [Header("Shift settings")]
    [SerializeField] private List<string> shiftEnabledRoomIds = new() { GameManager.HallwayRoomId, GameManager.BedroomRoomId };

    [Header("Timings")]
    [SerializeField] private float feedbackDuration = 2f;
    [SerializeField] private float roomTransitionDuration = 0.35f;
    [SerializeField] private float realityPulseDuration = 0.25f;

    private PanelRenderer.VersionedUIReloadCallback reloadCallback;

    private readonly List<VisualElement> inventorySlots = new();
    private Label feedbackText;
    private VisualElement shiftButton;
    private VisualElement transitionOverlay;
    private VisualElement realityPulse;

    private Coroutine feedbackRoutine;
    private Coroutine transitionRoutine;
    private Coroutine realityPulseRoutine;

    private bool inventoryEventsSubscribed;
    private bool roomEventsSubscribed;
    private bool realityEventsSubscribed;
    private bool shiftClickBound;

    private void OnEnable()
    {
        ResolvePanelRenderer();
        if (panelRenderer == null)
        {
            Debug.LogError("[HUDController] PanelRenderer component not found.");
            return;
        }

        if (panelRenderer.panelSettings == null)
        {
            Debug.LogError("[HUDController] PanelRenderer has no PanelSettings assigned.");
        }

        if (panelRenderer.visualTreeAsset == null)
        {
            Debug.LogError("[HUDController] PanelRenderer has no Source Asset (UXML) assigned.");
        }

        reloadCallback = HandlePanelReloaded;
        panelRenderer.RegisterUIReloadCallback(reloadCallback);

        TrySubscribeRuntimeEvents();
        InvokeRepeating(nameof(TrySubscribeRuntimeEvents), 0.5f, 0.5f);
    }

    private void Start()
    {
        ResolvePanelRenderer();
        TrySubscribeRuntimeEvents();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(TrySubscribeRuntimeEvents));

        if (panelRenderer != null && reloadCallback != null)
        {
            panelRenderer.UnregisterUIReloadCallback(reloadCallback);
        }

        if (shiftButton != null && shiftClickBound)
        {
            shiftButton.UnregisterCallback<ClickEvent>(OnShiftButtonClicked);
            shiftClickBound = false;
        }

        UnsubscribeRuntimeEvents();
    }

    public void ShowFeedback(string message, float duration = -1f)
    {
        if (feedbackText == null)
        {
            return;
        }

        var safeMessage = string.IsNullOrWhiteSpace(message) ? string.Empty : message.Trim();
        feedbackText.text = safeMessage;

        if (string.IsNullOrEmpty(safeMessage))
        {
            feedbackText.RemoveFromClassList("visible");
            return;
        }

        feedbackText.AddToClassList("visible");

        if (feedbackRoutine != null)
        {
            StopCoroutine(feedbackRoutine);
        }

        var targetDuration = duration > 0f ? duration : feedbackDuration;
        feedbackRoutine = StartCoroutine(HideFeedbackAfterDelay(targetDuration));
    }

    public void RefreshInventory()
    {
        if (inventorySlots.Count == 0)
        {
            return;
        }

        var itemList = InventoryManager.Instance?.GetItems() ?? Array.Empty<InventoryItemData>();
        for (var index = 0; index < inventorySlots.Count; index++)
        {
            var slot = inventorySlots[index];
            if (slot == null)
            {
                continue;
            }

            var item = index < itemList.Count ? itemList[index] : null;
            slot.userData = item != null ? item.id : null;

            var isFull = item != null;
            slot.EnableInClassList("full", isFull);
            slot.EnableInClassList("empty", !isFull);

            var label = slot.Q<Label>("slot-label");
            if (label == null)
            {
                label = new Label();
                label.name = "slot-label";
                label.AddToClassList("slot-label");
                slot.Add(label);
            }

            var isSelected = item != null &&
                !string.IsNullOrEmpty(InventoryManager.Instance?.SelectedItemId) &&
                item.id == InventoryManager.Instance.SelectedItemId;
            slot.EnableInClassList("selected", isSelected);
            label.text = item == null ? string.Empty : GetSlotLabel(item);
            label.style.display = item == null ? DisplayStyle.None : DisplayStyle.Flex;

            var isLast = index == inventorySlots.Count - 1;
            slot.style.marginRight = isLast ? 0 : 12;
            slot.pickingMode = item == null ? PickingMode.Ignore : PickingMode.Position;
            slot.focusable = item != null;
        }
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

        return item.id != null && item.id.Length > 0 ? item.id.Substring(0, 1).ToUpperInvariant() : "I";
    }

    private void OnInventorySlotClicked(ClickEvent evt)
    {
        var target = evt.target as VisualElement;
        var slot = target;

        while (slot != null && !slot.ClassListContains("inventory-slot"))
        {
            slot = slot.parent;
        }

        if (slot == null || string.IsNullOrEmpty(slot.userData as string))
        {
            return;
        }

        var itemId = slot.userData as string;
        var item = InventoryManager.Instance?.GetItemById(itemId);
        if (item == null)
        {
            return;
        }

        InventoryManager.Instance.SelectItem(itemId);
        ShowFeedback($"{item.displayName} selected.", 1.1f);
        RefreshInventory();
    }

    private void ResolvePanelRenderer()
    {
        if (panelRenderer == null)
        {
            panelRenderer = GetComponent<PanelRenderer>();
        }
    }

    private void HandlePanelReloaded(PanelRenderer renderer, VisualElement root, int _)
    {
        if (root == null)
        {
            Debug.LogError("[HUDController] Panel reloaded but root is null.");
            return;
        }

        inventorySlots.Clear();
        inventorySlots.Add(root.Q<VisualElement>("slot-0"));
        inventorySlots.Add(root.Q<VisualElement>("slot-1"));
        inventorySlots.Add(root.Q<VisualElement>("slot-2"));

        feedbackText = root.Q<Label>("feedback-text");
        transitionOverlay = root.Q<VisualElement>("transition-overlay");
        realityPulse = root.Q<VisualElement>("reality-pulse");

        if (shiftButton != null && shiftClickBound)
        {
            shiftButton.UnregisterCallback<ClickEvent>(OnShiftButtonClicked);
            shiftClickBound = false;
        }

        shiftButton = root.Q<VisualElement>("shift-button");
        if (shiftButton == null)
        {
            Debug.LogError("[HUDController] VisualElement 'shift-button' not found in active visual tree.");
        }
        else
        {
            shiftButton.RegisterCallback<ClickEvent>(OnShiftButtonClicked);
            shiftClickBound = true;
        }

        foreach (var slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.UnregisterCallback<ClickEvent>(OnInventorySlotClicked);
                slot.RegisterCallback<ClickEvent>(OnInventorySlotClicked);
            }
        }

        if (feedbackText != null)
        {
            feedbackText.pickingMode = PickingMode.Ignore;
        }

        if (transitionOverlay != null)
        {
            transitionOverlay.pickingMode = PickingMode.Ignore;
        }

        if (realityPulse != null)
        {
            realityPulse.pickingMode = PickingMode.Ignore;
        }

        RefreshInventory();
        RefreshShiftButtonState();
    }

    private void TrySubscribeRuntimeEvents()
    {
        if (!inventoryEventsSubscribed && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ItemAdded += HandleItemAdded;
            InventoryManager.Instance.ItemRemoved += HandleItemRemoved;
            inventoryEventsSubscribed = true;
        }

        if (!roomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged += HandleRoomChanged;
            roomEventsSubscribed = true;
        }

        if (!realityEventsSubscribed && RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged += HandleRealityChanged;
            realityEventsSubscribed = true;
        }

        RefreshShiftButtonState();
        RefreshInventory();
    }

    private void UnsubscribeRuntimeEvents()
    {
        if (inventoryEventsSubscribed && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ItemAdded -= HandleItemAdded;
            InventoryManager.Instance.ItemRemoved -= HandleItemRemoved;
        }

        if (roomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }

        if (realityEventsSubscribed && RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged -= HandleRealityChanged;
        }

        inventoryEventsSubscribed = false;
        roomEventsSubscribed = false;
        realityEventsSubscribed = false;
    }

    private void HandleItemAdded(InventoryItemData _)
    {
        RefreshInventory();
    }

    private void HandleItemRemoved(InventoryItemData _)
    {
        RefreshInventory();
    }

    private void HandleRoomChanged(RoomData _)
    {
        RefreshShiftButtonState();
        PlayTransitionOverlay();
    }

    private void HandleRealityChanged(RealityType _)
    {
        RefreshShiftButtonState();
        PlayRealityPulse();

        if (RealityManager.Instance != null)
        {
            var label = RealityManager.Instance.CurrentReality == RealityType.OtherSide ? "Other Side" : "Normal";
            ShowFeedback($"Reality: {label}.", 1.25f);
        }
    }

    private void OnShiftButtonClicked(ClickEvent _)
    {
        if (RealityManager.Instance == null)
        {
            Debug.LogError("[HUDController] RealityManager instance not found.");
            return;
        }

        var allowed = RealityManager.Instance.TryToggleReality();
        if (!allowed)
        {
            var roomId = RoomManager.Instance?.CurrentRoom?.id ?? "<unknown>";
            Debug.LogWarning($"[HUDController] SHIFT unavailable in room '{roomId}'.");
        }
    }

    private void RefreshShiftButtonState()
    {
        if (shiftButton == null)
        {
            return;
        }

        var allowed = IsShiftAllowedInCurrentRoom();
        shiftButton.SetEnabled(allowed);
        shiftButton.pickingMode = allowed ? PickingMode.Position : PickingMode.Ignore;
        shiftButton.focusable = allowed;

        var isOtherSide = RealityManager.Instance != null && RealityManager.Instance.CurrentReality == RealityType.OtherSide;
        shiftButton.EnableInClassList("normal", !isOtherSide);
        shiftButton.EnableInClassList("other-side", isOtherSide);
    }

    private bool IsShiftAllowedInCurrentRoom()
    {
        if (RealityManager.Instance != null)
        {
            return RealityManager.Instance.IsShiftAllowedInCurrentRoom();
        }

        var roomId = RoomManager.Instance?.CurrentRoom?.id;
        if (string.IsNullOrWhiteSpace(roomId))
        {
            return false;
        }

        foreach (var allowedRoomId in shiftEnabledRoomIds)
        {
            if (!string.IsNullOrWhiteSpace(allowedRoomId) &&
                roomId.Equals(allowedRoomId, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private void PlayTransitionOverlay()
    {
        if (transitionOverlay == null)
        {
            return;
        }

        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);
        }

        transitionRoutine = StartCoroutine(PlayOverlayRoutine(transitionOverlay, roomTransitionDuration));
    }

    private void PlayRealityPulse()
    {
        if (realityPulse == null)
        {
            return;
        }

        if (realityPulseRoutine != null)
        {
            StopCoroutine(realityPulseRoutine);
        }

        realityPulseRoutine = StartCoroutine(PlayOverlayRoutine(realityPulse, realityPulseDuration));
    }

    private IEnumerator HideFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null)
        {
            feedbackText.RemoveFromClassList("visible");
        }
    }

    private IEnumerator PlayOverlayRoutine(VisualElement element, float duration)
    {
        element.AddToClassList("active");
        yield return new WaitForSeconds(duration);
        element.RemoveFromClassList("active");
    }
}
