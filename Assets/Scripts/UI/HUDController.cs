using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum StoryOverlayMode
{
    Letter,
    Ending,
    Intro
}

[RequireComponent(typeof(PanelRenderer))]
public class HUDController : MonoBehaviour
{
    [Header("Runtime References")]
    [SerializeField] private PanelRenderer panelRenderer;

    [Header("Lantern icon")]
    [SerializeField] private Sprite lanternNotObtainedSprite;
    [SerializeField] private Sprite lanternObtainedSprite;
    [SerializeField] private string lanternItemId = "flashlight_tool";

    [Header("Inventory item icons")]
    [SerializeField] private Sprite staffKeyItemIcon;
    [SerializeField] private Sprite libraryKeyItemIcon;
    [SerializeField] private Sprite wornBookItemIcon;
    [SerializeField] private Sprite floppyDiskItemIcon;
    [SerializeField] private Sprite basementCodeItemIcon;
    [SerializeField] private Sprite valveHandleItemIcon;

    [Header("Memory discovery concepts")]
    [SerializeField] private Sprite memory01Concept;
    [SerializeField] private Sprite memory02Concept;
    [SerializeField] private Sprite memory03Concept;
    [SerializeField] private Sprite memory04Concept;
    [SerializeField] private Sprite memory05Concept;

    [Header("Cursor")]
    [SerializeField] private Texture2D customCursorTexture;
    [SerializeField] private Vector2 customCursorHotspot = Vector2.zero;

    [Header("Timings")]
    [SerializeField] private float feedbackDuration = 2f;
    [SerializeField] private float roomTransitionDuration = 0.35f;
    [SerializeField] private float realityPulseDuration = 0.25f;
    [SerializeField] private float memoryDiscoveryDuration = 10f;

    private PanelRenderer.VersionedUIReloadCallback reloadCallback;
    private Label feedbackText;
    private Label hoverNameText;
    private VisualElement shiftButton;
    private VisualElement lanternIcon;
    private VisualElement transitionOverlay;
    private VisualElement realityPulse;
    private VisualElement inventoryPanel;
    private VisualElement hudRoot;

    private Coroutine feedbackRoutine;
    private Coroutine transitionRoutine;
    private Coroutine realityPulseRoutine;

    private bool inventoryEventsSubscribed;
    private bool roomEventsSubscribed;
    private bool realityEventsSubscribed;
    private bool shiftClickBound;

    private HUDInventoryView inventoryView;
    private HUDStoryOverlayView storyView;
    private HUDMemoryDiscoveryView memoryView;

    private void OnEnable()
    {
        UnityEngine.Cursor.SetCursor(
            customCursorTexture,
            customCursorHotspot,
            CursorMode.Auto);
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
        if (storyView != null)
        {
            storyView.Update();
        }
        if (memoryView != null)
        {
            memoryView.Update();
        }
        UpdateInteractableHover();
    }

    private void UpdateInteractableHover()
    {
        if (hoverNameText == null)
        {
            return;
        }

        var name = ResolveHoveredInteractableName();
        if (string.IsNullOrWhiteSpace(name))
        {
            hoverNameText.text = string.Empty;
            hoverNameText.style.opacity = 0f;
            return;
        }

        hoverNameText.text = name;
        hoverNameText.style.opacity = 1f;
    }

    private static string ResolveHoveredInteractableName()
    {
        if (UIManager.Instance != null && UIManager.Instance.IsStoryOverlayVisible())
        {
            return string.Empty;
        }

        var mouse = Mouse.current;
        var camera = Camera.main;
        if (mouse == null || camera == null)
        {
            return string.Empty;
        }

        var ray = camera.ScreenPointToRay(mouse.position.ReadValue());
        var hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        for (var index = 0; index < hits.Length; index++)
        {
            var behaviour = hits[index].collider.GetComponent<InteractableBehaviour>();
            if (behaviour == null)
            {
                behaviour = hits[index].collider.GetComponentInParent<InteractableBehaviour>();
            }
            if (behaviour == null || !behaviour.HasData())
            {
                continue;
            }

            var data = behaviour.GetData();
            if (RoomManager.Instance != null
                && !RoomManager.Instance.IsInteractableVisible(data))
            {
                continue;
            }

            return data.displayName;
        }

        return string.Empty;
    }

    private void OnDisable()
    {
        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
        feedbackRoutine = StartCoroutine(HideFeedbackAfterDelay(duration > 0f ? duration : feedbackDuration));
    }

    public void ShowStoryOverlay(string title, string body, string hint = "Continue", StoryOverlayMode mode = StoryOverlayMode.Letter)
    {
        if (storyView != null)
        {
            if (mode == StoryOverlayMode.Intro)
            {
                storyView.ShowIntro(title, body, hint);
            }
            else
            {
                storyView.ShowLetter(title, body, hint);
            }
        }
    }

    public void ShowIntro(string title, string body, string hint = "Continue")
    {
        if (storyView != null)
        {
            storyView.ShowIntro(title, body, hint);
        }
    }

    public void ShowMemoryDiscovery(MemoryDefinition memory)
    {
        if (memoryView != null)
        {
            memoryView.Show(memory);
        }
    }

    public void ShowEndingOverlay(string title, string farewell, string credits, int memoryCount)
    {
        if (storyView != null)
        {
            storyView.ShowEnding(title, farewell, credits, memoryCount);
        }
    }

    public void HideStoryOverlay()
    {
        if (storyView != null)
        {
            storyView.Hide();
        }
    }

    public bool IsStoryOverlayVisible()
    {
        return storyView != null && storyView.IsVisible;
    }

    public void SetGameplayControlsVisible(bool visible)
    {
        var isMainMenu = RoomManager.Instance != null
            && RoomManager.Instance.CurrentRoom != null
            && RoomManager.Instance.CurrentRoom.id == GameManager.MainMenuRoomId;

        if (inventoryPanel != null)
        {
            inventoryPanel.style.display = visible && !isMainMenu ? DisplayStyle.Flex : DisplayStyle.None;
        }
        if (feedbackText != null)
        {
            feedbackText.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        if (shiftButton != null)
        {
            shiftButton.style.display = visible && !isMainMenu ? DisplayStyle.Flex : DisplayStyle.None;
        }
        if (lanternIcon != null)
        {
            lanternIcon.style.display = visible && !isMainMenu ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public void RefreshInventory()
    {
        if (inventoryView == null)
        {
            return;
        }
        inventoryView.Refresh();
        RefreshLanternState();
    }

    public void RefreshLanternState()
    {
        if (inventoryView == null)
        {
            return;
        }
        inventoryView.RefreshLantern(
            lanternIcon,
            InventoryManager.Instance,
            GameManager.Instance,
            lanternItemId,
            lanternNotObtainedSprite,
            lanternObtainedSprite);
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
        inventoryPanel = root.Q<VisualElement>("inventory-panel");
        feedbackText = root.Q<Label>("feedback-text");
        hoverNameText = root.Q<Label>("hover-name-text");
        if (hoverNameText == null)
        {
            hoverNameText = new Label
            {
                name = "hover-name-text",
                text = string.Empty
            };
            hoverNameText.AddToClassList("hover-name-text");
            hoverNameText.pickingMode = PickingMode.Ignore;
            root.Add(hoverNameText);
        }
        transitionOverlay = root.Q<VisualElement>("transition-overlay");
        realityPulse = root.Q<VisualElement>("reality-pulse");

        inventoryView = new HUDInventoryView(
            this,
            root,
            ShowFeedback);
        inventoryView.SetItemIcons(
            staffKeyItemIcon,
            libraryKeyItemIcon,
            wornBookItemIcon,
            floppyDiskItemIcon,
            basementCodeItemIcon,
            valveHandleItemIcon);
        memoryView = new HUDMemoryDiscoveryView(
            this,
            hudRoot,
            memory01Concept,
            memory02Concept,
            memory03Concept,
            memory04Concept,
            memory05Concept,
            memoryDiscoveryDuration);
        storyView = new HUDStoryOverlayView(this, hudRoot);

        BindShiftButton(root);
        inventoryView.Bind();

        lanternIcon = root.Q<VisualElement>("lantern-icon");
        if (lanternIcon != null)
        {
            lanternIcon.pickingMode = PickingMode.Ignore;
            lanternIcon.focusable = false;
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
        SetGameplayControlsVisible(true);
    }

    private void BindShiftButton(VisualElement root)
    {
        if (shiftButton != null && shiftClickBound)
        {
            shiftButton.UnregisterCallback<ClickEvent>(OnShiftButtonClicked);
            shiftClickBound = false;
        }

        shiftButton = root.Q<VisualElement>("shift-button");
        if (shiftButton == null)
        {
            Debug.LogError("[HUDController] VisualElement 'shift-button' not found in active visual tree.");
            return;
        }

        shiftButton.RegisterCallback<ClickEvent>(OnShiftButtonClicked);
        shiftClickBound = true;
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

    private void HandleItemAdded(InventoryItemData _) => RefreshInventory();
    private void HandleItemRemoved(InventoryItemData _) => RefreshInventory();

    private void HandleRoomChanged(RoomData _)
    {
        RefreshShiftButtonState();
        SetGameplayControlsVisible(true);
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
        RealityManager.Instance.TryToggleReality();
    }

    private void RefreshShiftButtonState()
    {
        if (shiftButton == null)
        {
            return;
        }
        var allowed = RealityManager.Instance != null
            ? RealityManager.Instance.IsShiftAllowedInCurrentRoom()
            : false;
        shiftButton.SetEnabled(allowed);
        shiftButton.pickingMode = allowed ? PickingMode.Position : PickingMode.Ignore;
        shiftButton.focusable = allowed;

        var isOtherSide = RealityManager.Instance != null
            && RealityManager.Instance.CurrentReality == RealityType.OtherSide;
        shiftButton.EnableInClassList("normal", !isOtherSide);
        shiftButton.EnableInClassList("other-side", isOtherSide);
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
        feedbackText.RemoveFromClassList("visible");
    }

    private static IEnumerator PlayOverlayRoutine(VisualElement element, float duration)
    {
        element.AddToClassList("active");
        yield return new WaitForSeconds(duration);
        element.RemoveFromClassList("active");
    }
}
