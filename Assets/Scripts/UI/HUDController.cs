using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum StoryOverlayMode
{
    Letter,
    Ending
}

[RequireComponent(typeof(PanelRenderer))]
public class HUDController : MonoBehaviour
{
    [Header("Runtime References")]
    [SerializeField] private PanelRenderer panelRenderer;

    [Header("Shift settings")]
    [SerializeField] private List<string> shiftEnabledRoomIds = new() { GameManager.HallwayRoomId, GameManager.BedroomRoomId };

    [Header("Lantern icon")]
    [SerializeField] private Sprite lanternNotObtainedSprite;
    [SerializeField] private Sprite lanternObtainedSprite;
    [SerializeField] private string lanternItemId = "flashlight_tool";

    [Header("Timings")]
    [SerializeField] private float feedbackDuration = 2f;
    [SerializeField] private float roomTransitionDuration = 0.35f;
    [SerializeField] private float realityPulseDuration = 0.25f;

    private PanelRenderer.VersionedUIReloadCallback reloadCallback;

    private readonly List<VisualElement> inventorySlots = new();
    private Label feedbackText;
    private VisualElement shiftButton;
    private VisualElement lanternIcon;
    private VisualElement transitionOverlay;
    private VisualElement realityPulse;
    private VisualElement inventoryPanel;
    private VisualElement storyOverlay;
    private VisualElement storyOverlayBackdrop;
    private VisualElement storyOverlayPanel;
    private Label storyOverlayEyebrow;
    private Label storyOverlayTitle;
    private Label storyOverlayBody;
    private Label storyOverlayMemoryCount;
    private Label storyOverlayCredits;
    private Button storyOverlayClose;
    private Button storyOverlayRestart;
    private VisualElement hudRoot;

    private Coroutine feedbackRoutine;
    private Coroutine transitionRoutine;
    private Coroutine realityPulseRoutine;

    private bool inventoryEventsSubscribed;
    private bool roomEventsSubscribed;
    private bool realityEventsSubscribed;
    private bool shiftClickBound;
    private bool storyEventsBound;
    private StoryOverlayMode storyOverlayMode = StoryOverlayMode.Letter;

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

    private void Update()
    {
        if (storyOverlay == null || !storyOverlay.ClassListContains("visible"))
        {
            return;
        }

        var keyboard = Keyboard.current;
        if (keyboard != null
            && (keyboard.escapeKey.wasPressedThisFrame
                || keyboard.enterKey.wasPressedThisFrame
                || keyboard.spaceKey.wasPressedThisFrame))
        {
            if (storyOverlayMode == StoryOverlayMode.Letter)
            {
                CloseStoryOverlay();
            }
            else
            {
                RestartGame();
            }

            return;
        }

        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            var panelPoint = GetHudMousePoint(mouse.position.ReadValue());
            if (storyOverlayMode == StoryOverlayMode.Letter
                && storyOverlayClose != null
                && storyOverlayClose.worldBound.Contains(panelPoint))
            {
                CloseStoryOverlay();
            }
            else if (storyOverlayMode == StoryOverlayMode.Ending
                && storyOverlayRestart != null
                && storyOverlayRestart.worldBound.Contains(panelPoint))
            {
                RestartGame();
            }
        }
    }

    private Vector2 GetHudMousePoint(Vector2 screenPoint)
    {
        var hudWidth = hudRoot != null ? hudRoot.worldBound.width : 640f;
        var hudHeight = hudRoot != null ? hudRoot.worldBound.height : 360f;
        if (hudWidth <= 0f)
        {
            hudWidth = 640f;
        }

        if (hudHeight <= 0f)
        {
            hudHeight = 360f;
        }

        var x = screenPoint.x * hudWidth / Mathf.Max(Screen.width, 1);
        var y = (Screen.height - screenPoint.y) * hudHeight / Mathf.Max(Screen.height, 1);
        return new Vector2(x, y);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(TrySubscribeRuntimeEvents));

        UnbindStoryOverlayEvents();

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

    public void ShowStoryOverlay(string title, string body, string hint = "Continue", StoryOverlayMode mode = StoryOverlayMode.Letter)
    {
        ResolvePanelRenderer();
        BuildStoryOverlay(true);
        BindStoryOverlayEvents();
        if (storyOverlay == null)
        {
            Debug.LogWarning("[HUDController] Story overlay is not present in the active visual tree.");
            return;
        }

        storyOverlayMode = mode;
        if (storyOverlayEyebrow != null)
        {
            storyOverlayEyebrow.text = mode == StoryOverlayMode.Letter
                ? "A LETTER"
                : "ENDING";
        }

        if (storyOverlayTitle != null)
        {
            storyOverlayTitle.text = title;
        }

        if (storyOverlayBody != null)
        {
            storyOverlayBody.text = body;
        }

        if (storyOverlayClose != null)
        {
            storyOverlayClose.text = hint;
            storyOverlayClose.style.display = DisplayStyle.Flex;
            storyOverlayClose.pickingMode = PickingMode.Position;
            storyOverlayClose.SetEnabled(true);
            storyOverlayClose.focusable = true;
        }

        if (storyOverlayRestart != null)
        {
            storyOverlayRestart.style.display = mode == StoryOverlayMode.Ending
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            storyOverlayRestart.SetEnabled(mode == StoryOverlayMode.Ending);
            storyOverlayRestart.focusable = mode == StoryOverlayMode.Ending;
        }

        if (storyOverlayMemoryCount != null)
        {
            storyOverlayMemoryCount.text = mode == StoryOverlayMode.Ending
                ? storyOverlayMemoryCount.text
                : string.Empty;
        }

        if (storyOverlayCredits != null)
        {
            storyOverlayCredits.text = string.Empty;
        }

        SetGameplayControlsVisible(false);
        ExpandStoryOverlayToHud();
        storyOverlay.style.display = DisplayStyle.Flex;
        storyOverlay.AddToClassList("visible");
        if (storyOverlayClose != null && storyOverlayClose.resolvedStyle.display == DisplayStyle.Flex)
        {
            storyOverlayClose.Focus();
        }
    }

    public void ShowEndingOverlay(string title, string farewell, string credits, int memoryCount)
    {
        ResolvePanelRenderer();
        BuildStoryOverlay(true);
        BindStoryOverlayEvents();
        if (storyOverlay == null)
        {
            Debug.LogWarning("[HUDController] Story overlay is not present in the active visual tree.");
            return;
        }

        storyOverlayMode = StoryOverlayMode.Ending;
        if (storyOverlayEyebrow != null)
        {
            storyOverlayEyebrow.text = "ENDING";
        }

        if (storyOverlayTitle != null)
        {
            storyOverlayTitle.text = title;
        }

        if (storyOverlayBody != null)
        {
            storyOverlayBody.text = farewell;
        }

        if (storyOverlayMemoryCount != null)
        {
            storyOverlayMemoryCount.text = $"Recovered Memories: {Mathf.Clamp(memoryCount, 0, 5)} / 5";
        }

        if (storyOverlayCredits != null)
        {
            storyOverlayCredits.text = credits;
        }

        if (storyOverlayClose != null)
        {
            storyOverlayClose.style.display = DisplayStyle.None;
            storyOverlayClose.SetEnabled(false);
            storyOverlayClose.focusable = false;
        }

        if (storyOverlayRestart != null)
        {
            storyOverlayRestart.style.display = DisplayStyle.Flex;
            storyOverlayRestart.text = "Restart game";
            storyOverlayRestart.pickingMode = PickingMode.Position;
            storyOverlayRestart.SetEnabled(true);
            storyOverlayRestart.focusable = true;
        }

        SetGameplayControlsVisible(false);
        ExpandStoryOverlayToHud();
        storyOverlay.style.display = DisplayStyle.Flex;
        storyOverlay.AddToClassList("visible");
        if (storyOverlayRestart != null)
        {
            storyOverlayRestart.Focus();
        }
    }

    public void HideStoryOverlay()
    {
        if (storyOverlay != null)
        {
            storyOverlay.RemoveFromClassList("visible");
            storyOverlay.style.display = DisplayStyle.None;
        }
    }

    public bool IsStoryOverlayVisible()
    {
        return storyOverlay != null && storyOverlay.ClassListContains("visible");
    }

    private void ExpandStoryOverlayToHud()
    {
        if (storyOverlay == null)
        {
            return;
        }

        var width = hudRoot != null ? hudRoot.worldBound.width : 0f;
        var height = hudRoot != null ? hudRoot.worldBound.height : 0f;
        if (width <= 0f || height <= 0f)
        {
            width = 640f;
            height = 360f;
        }

        storyOverlay.style.left = 0f;
        storyOverlay.style.top = 0f;
        storyOverlay.style.position = Position.Absolute;
        storyOverlay.style.alignItems = Align.Center;
        storyOverlay.style.justifyContent = Justify.Center;
        storyOverlay.style.width = width;
        storyOverlay.style.height = height;

        if (storyOverlayPanel != null)
        {
            var panelWidth = Mathf.Clamp(width * 0.58f, 420f, 620f);
            var panelHeight = Mathf.Min(300f, Mathf.Max(220f, height - 40f));
            storyOverlayPanel.style.position = Position.Absolute;
            storyOverlayPanel.style.left = (width - panelWidth) * 0.5f;
            storyOverlayPanel.style.top = (height - panelHeight) * 0.5f;
            storyOverlayPanel.style.width = panelWidth;
            storyOverlayPanel.style.height = panelHeight;
            storyOverlayPanel.style.minHeight = 0f;
            storyOverlayPanel.style.maxHeight = panelHeight;
        }
    }

    public void SetGameplayControlsVisible(bool visible)
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        if (feedbackText != null)
        {
            feedbackText.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        if (shiftButton != null)
        {
            shiftButton.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        if (lanternIcon != null)
        {
            lanternIcon.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public void RefreshInventory()
    {
        if (inventorySlots.Count == 0)
        {
            return;
        }

        IReadOnlyList<InventoryItemData> itemList = Array.Empty<InventoryItemData>();
        if (InventoryManager.Instance != null)
        {
            itemList = InventoryManager.Instance.GetItems();
        }
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
                InventoryManager.Instance != null &&
                !string.IsNullOrEmpty(InventoryManager.Instance.SelectedItemId) &&
                item.id == InventoryManager.Instance.SelectedItemId;
            slot.EnableInClassList("selected", isSelected);
            label.text = item == null ? string.Empty : GetSlotLabel(item);
            label.style.display = item == null ? DisplayStyle.None : DisplayStyle.Flex;

            var isLast = index == inventorySlots.Count - 1;
            slot.style.marginRight = isLast ? 0 : 12;
            slot.pickingMode = item == null ? PickingMode.Ignore : PickingMode.Position;
            slot.focusable = item != null;
        }

        RefreshLanternState();
    }

    public void RefreshLanternState()
    {
        if (lanternIcon == null)
        {
            return;
        }

        var hasLantern = InventoryManager.Instance != null && InventoryManager.Instance.HasItem(lanternItemId);
        if (!hasLantern && GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.HasFlashlight))
        {
            hasLantern = true;
        }

        var selectedSprite = hasLantern ? (lanternObtainedSprite != null ? lanternObtainedSprite : lanternNotObtainedSprite) : lanternNotObtainedSprite;
        lanternIcon.style.backgroundImage = selectedSprite != null ? new StyleBackground(selectedSprite) : new StyleBackground();
        lanternIcon.style.opacity = hasLantern ? 1f : 0.9f;
        lanternIcon.pickingMode = PickingMode.Ignore;
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
        InventoryItemData item = null;
        if (InventoryManager.Instance != null)
        {
            item = InventoryManager.Instance.GetItemById(itemId);
        }
        if (item == null)
        {
            return;
        }

        InventoryManager.Instance.SelectItem(itemId);
        ShowFeedback($"{item.displayName} selected.", 1.1f);
        RefreshInventory();
    }

    private void OnInventorySlotContextClicked(ContextClickEvent evt)
    {
        var slot = ResolveInventorySlot(evt.target as VisualElement);
        if (slot == null || string.IsNullOrEmpty(slot.userData as string))
        {
            return;
        }

        InspectInventoryItem(slot.userData as string, evt);
    }

    private void OnInventorySlotMouseDown(MouseDownEvent evt)
    {
        if (evt.button != (int)MouseButton.RightMouse)
        {
            return;
        }

        var slot = ResolveInventorySlot(evt.target as VisualElement);
        if (slot == null || string.IsNullOrEmpty(slot.userData as string))
        {
            return;
        }

        InspectInventoryItem(slot.userData as string, evt);
    }

    private VisualElement ResolveInventorySlot(VisualElement target)
    {
        var slot = target;
        while (slot != null && !slot.ClassListContains("inventory-slot"))
        {
            slot = slot.parent;
        }

        return slot;
    }

    private void InspectInventoryItem(string itemId, EventBase evt)
    {
        InventoryItemData item = null;
        if (InventoryManager.Instance != null)
        {
            item = InventoryManager.Instance.GetItemById(itemId);
        }
        if (item == null)
        {
            return;
        }

        var currentReality = RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal;
        var description = item.GetDescriptionForReality(currentReality);

        if (item.id.Equals("worn_book", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null
            && !GameManager.Instance.HasFlag(GameFlagNames.FloppyDiskRecovered))
        {
            var floppyDisk = new InventoryItemData
            {
                id = "floppy_disk",
                displayName = "Floppy Disk",
                description = "A small disk hidden inside the book.",
                descriptionInNormalReality = "A small, faded floppy disk has been tucked inside the back pages of the Worn Book.",
                descriptionInOtherReality = "The disk is not just hidden: it looks like it was placed there to be found later by someone from this reality.",
                isKeyItem = true
            };

            if (!InventoryManager.Instance.HasItem(floppyDisk.id))
            {
                InventoryManager.Instance.AddItem(floppyDisk);
                GameManager.Instance.SetFlag(GameFlagNames.FloppyDiskRecovered, true);
                description = "You inspect the Worn Book and discover a hidden floppy disk tucked into the back pages.";
            }
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            description = $"You inspect {item.displayName}.";
        }

        ShowFeedback(description, 2f);
        RefreshInventory();
        evt.StopPropagation();
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

        hudRoot = root.Q<VisualElement>("hud-root");
        inventorySlots.Clear();
        inventorySlots.Add(root.Q<VisualElement>("slot-0"));
        inventorySlots.Add(root.Q<VisualElement>("slot-1"));
        inventorySlots.Add(root.Q<VisualElement>("slot-2"));

        inventoryPanel = root.Q<VisualElement>("inventory-panel");
        feedbackText = root.Q<Label>("feedback-text");
        transitionOverlay = root.Q<VisualElement>("transition-overlay");
        realityPulse = root.Q<VisualElement>("reality-pulse");
        BuildStoryOverlay(false);
        BindStoryOverlayEvents();

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

        lanternIcon = root.Q<VisualElement>("lantern-icon");
        if (lanternIcon != null)
        {
            lanternIcon.pickingMode = PickingMode.Ignore;
            lanternIcon.focusable = false;
        }

        foreach (var slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.UnregisterCallback<ClickEvent>(OnInventorySlotClicked);
                slot.RegisterCallback<ClickEvent>(OnInventorySlotClicked);
                slot.UnregisterCallback<ContextClickEvent>(OnInventorySlotContextClicked);
                slot.RegisterCallback<ContextClickEvent>(OnInventorySlotContextClicked);
                slot.UnregisterCallback<MouseDownEvent>(OnInventorySlotMouseDown);
                slot.RegisterCallback<MouseDownEvent>(OnInventorySlotMouseDown);
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
        RefreshLanternState();
    }

    private void BuildStoryOverlay(bool visible)
    {
        if (hudRoot == null)
        {
            return;
        }

        if (storyEventsBound)
        {
            UnbindStoryOverlayEvents();
        }

        if (storyOverlay != null)
        {
            storyOverlay.RemoveFromHierarchy();
        }

        storyOverlay = new VisualElement { name = "hud-modal-layer" };

        storyOverlayBackdrop = new VisualElement { name = "story-overlay-backdrop" };
        storyOverlayBackdrop.AddToClassList("story-overlay-backdrop");
        storyOverlayBackdrop.pickingMode = PickingMode.Ignore;

        storyOverlayPanel = new VisualElement { name = "story-overlay-panel" };
        storyOverlayPanel.AddToClassList("story-overlay-panel");

        storyOverlayEyebrow = CreateStoryLabel(
            "story-overlay-eyebrow",
            "story-overlay-eyebrow");
        storyOverlayTitle = CreateStoryLabel(
            "story-overlay-title",
            "story-overlay-title");
        storyOverlayBody = CreateStoryLabel(
            "story-overlay-body",
            "story-overlay-body");
        storyOverlayMemoryCount = CreateStoryLabel(
            "story-overlay-memory-count",
            "story-overlay-memory-count");
        storyOverlayCredits = CreateStoryLabel(
            "story-overlay-credits",
            "story-overlay-credits");

        var actions = new VisualElement { name = "story-overlay-actions" };
        actions.AddToClassList("story-overlay-actions");
        actions.style.position = Position.Absolute;
        actions.style.left = 0f;
        actions.style.right = 0f;
        actions.style.bottom = 18f;
        actions.style.height = 42f;

        storyOverlayClose = new Button { name = "story-overlay-close", text = "Continue" };
        storyOverlayClose.AddToClassList("story-overlay-close");
        storyOverlayClose.pickingMode = PickingMode.Position;
        storyOverlayClose.style.position = Position.Absolute;
        storyOverlayClose.style.bottom = 0f;
        storyOverlayClose.style.left = 122f;
        storyOverlayClose.style.width = 176f;
        storyOverlayClose.style.height = 42f;

        storyOverlayRestart = new Button { name = "story-overlay-restart", text = "Restart game" };
        storyOverlayRestart.AddToClassList("story-overlay-restart");
        storyOverlayRestart.pickingMode = PickingMode.Position;
        storyOverlayRestart.style.display = DisplayStyle.None;
        storyOverlayRestart.style.position = Position.Absolute;
        storyOverlayRestart.style.bottom = 0f;
        storyOverlayRestart.style.left = 122f;
        storyOverlayRestart.style.width = 176f;
        storyOverlayRestart.style.height = 42f;

        actions.Add(storyOverlayClose);
        actions.Add(storyOverlayRestart);
        storyOverlayPanel.Add(storyOverlayEyebrow);
        storyOverlayPanel.Add(storyOverlayTitle);
        storyOverlayPanel.Add(storyOverlayBody);
        storyOverlayPanel.Add(storyOverlayMemoryCount);
        storyOverlayPanel.Add(storyOverlayCredits);
        storyOverlayPanel.Add(actions);
        storyOverlay.Add(storyOverlayBackdrop);
        storyOverlay.Add(storyOverlayPanel);
        storyOverlay.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        hudRoot.Add(storyOverlay);
    }

    private static Label CreateStoryLabel(string name, string className)
    {
        var label = new Label { name = name };
        label.AddToClassList(className);
        return label;
    }

    private void BindStoryOverlayEvents()
    {
        if (storyEventsBound)
        {
            UnbindStoryOverlayEvents();
        }

        if (storyOverlayClose != null)
        {
            storyOverlayClose.RegisterCallback<PointerDownEvent>(OnStoryOverlayClosePointerDown);
        }

        if (storyOverlayRestart != null)
        {
            storyOverlayRestart.RegisterCallback<PointerDownEvent>(OnStoryOverlayRestartPointerDown);
        }

        if (hudRoot != null)
        {
            hudRoot.RegisterCallback<KeyDownEvent>(OnStoryOverlayKeyDown);
        }

        storyEventsBound = true;
    }

    private void UnbindStoryOverlayEvents()
    {
        if (storyOverlayClose != null)
        {
            storyOverlayClose.UnregisterCallback<PointerDownEvent>(OnStoryOverlayClosePointerDown);
        }

        if (storyOverlayRestart != null)
        {
            storyOverlayRestart.UnregisterCallback<PointerDownEvent>(OnStoryOverlayRestartPointerDown);
        }

        if (hudRoot != null)
        {
            hudRoot.UnregisterCallback<KeyDownEvent>(OnStoryOverlayKeyDown);
        }

        storyEventsBound = false;
    }

    private void OnStoryOverlayCloseClicked(ClickEvent _)
    {
        CloseStoryOverlay();
    }

    private void OnStoryOverlayClosePointerDown(PointerDownEvent _)
    {
        CloseStoryOverlay();
    }

    private void OnStoryOverlayRestartClicked(ClickEvent _)
    {
        RestartGame();
    }

    private void OnStoryOverlayRestartPointerDown(PointerDownEvent _)
    {
        RestartGame();
    }

    private void OnStoryOverlayKeyDown(KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Escape
            && storyOverlay != null
            && storyOverlay.ClassListContains("visible"))
        {
            if (storyOverlayMode == StoryOverlayMode.Letter)
            {
                CloseStoryOverlay();
            }

            evt.StopPropagation();
        }
    }

    private void CloseStoryOverlay()
    {
        if (storyOverlayMode == StoryOverlayMode.Letter)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetFlag(GameFlagNames.ForestEndingReached, true);
            }

            var memoryCount = CountRecoveredMemories();
            var ending = ResolveForestEnding(memoryCount);
            ShowEndingOverlay(
                ending.title,
                ending.farewell,
                "Game design and development",
                memoryCount);
            return;
        }

        HideStoryOverlay();
    }

    private static int CountRecoveredMemories()
    {
        if (GameManager.Instance == null)
        {
            return 0;
        }

        var count = 0;
        if (GameManager.Instance.HasFlag(GameFlagNames.Memory_01)) count++;
        if (GameManager.Instance.HasFlag(GameFlagNames.Memory_02)) count++;
        if (GameManager.Instance.HasFlag(GameFlagNames.Memory_03)) count++;
        if (GameManager.Instance.HasFlag(GameFlagNames.Memory_04)) count++;
        if (GameManager.Instance.HasFlag(GameFlagNames.Memory_05)) count++;
        return count;
    }

    private static (string title, string farewell) ResolveForestEnding(int memoryCount)
    {
        if (memoryCount <= 2)
        {
            return (
                "Bad Ending - Denial",
                "You are still not ready to leave the lie behind. The farewell remains unfinished.");
        }

        if (memoryCount <= 4)
        {
            return (
                "Neutral Ending - Escape",
                "You understood part of the truth, but there is still one word that could not be spoken.");
        }

        return (
            "True Ending - Reunion",
            "You accept his death and your own. At last, you can stop searching for answers and say goodbye.");
    }

    private void RestartGame()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        HideStoryOverlay();
        SetGameplayControlsVisible(true);
        GameManager.Instance.InitializeMVP();
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.LoadRoom(GameManager.LobbyRoomId);
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.SetReality(RealityType.Normal);
        }
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
        RefreshLanternState();
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
        RefreshLanternState();
    }

    private void HandleItemRemoved(InventoryItemData _)
    {
        RefreshInventory();
        RefreshLanternState();
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
            var roomId = "<unknown>";
            if (RoomManager.Instance != null && RoomManager.Instance.CurrentRoom != null)
            {
                roomId = RoomManager.Instance.CurrentRoom.id;
            }
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

        var roomId = string.Empty;
        if (RoomManager.Instance != null && RoomManager.Instance.CurrentRoom != null)
        {
            roomId = RoomManager.Instance.CurrentRoom.id;
        }
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
