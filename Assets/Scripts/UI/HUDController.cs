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
    [SerializeField] private List<string> shiftEnabledRoomIds = new() { GameManager.HallwayRoomId };

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

        var itemCount = InventoryManager.Instance?.GetItems().Count ?? 0;
        for (var index = 0; index < inventorySlots.Count; index++)
        {
            var slot = inventorySlots[index];
            if (slot == null)
            {
                continue;
            }

            var isFull = index < itemCount;
            slot.EnableInClassList("full", isFull);
            slot.EnableInClassList("empty", !isFull);

            var isLast = index == inventorySlots.Count - 1;
            slot.style.marginRight = isLast ? 0 : 12;
        }
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
        if (!IsShiftAllowedInCurrentRoom())
        {
            var roomId = RoomManager.Instance?.CurrentRoom?.id ?? "<unknown>";
            Debug.LogWarning($"[HUDController] SHIFT unavailable in room '{roomId}'.");
            ShowFeedback("Shift unavailable here.", 1.25f);
            return;
        }

        if (RealityManager.Instance == null)
        {
            Debug.LogError("[HUDController] RealityManager instance not found.");
            return;
        }

        RealityManager.Instance.ToggleReality();
        GameManager.Instance?.SetFlag(GameFlagNames.ShiftPerformed, true);
    }

    private void RefreshShiftButtonState()
    {
        if (shiftButton == null)
        {
            return;
        }

        var allowed = IsShiftAllowedInCurrentRoom();
        shiftButton.SetEnabled(allowed);

        var isOtherSide = RealityManager.Instance != null && RealityManager.Instance.CurrentReality == RealityType.OtherSide;
        shiftButton.EnableInClassList("normal", !isOtherSide);
        shiftButton.EnableInClassList("other-side", isOtherSide);
    }

    private bool IsShiftAllowedInCurrentRoom()
    {
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
