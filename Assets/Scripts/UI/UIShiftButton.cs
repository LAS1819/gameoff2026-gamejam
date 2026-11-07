using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Logic for the Shift icon. All visual appearance is handled in the UXML/USS stylesheet.
/// </summary>
[RequireComponent(typeof(PanelRenderer))]
public class UIShiftButton : MonoBehaviour
{
    [Header("Room gating")]
    [SerializeField] private List<string> shiftEnabledRoomIds = new() { GameManager.HallwayRoomId };

    private PanelRenderer m_PanelRenderer;
    private VisualElement m_ShiftButton;
    private PanelRenderer.VersionedUIReloadCallback m_ReloadCallback;
    private int m_LastBoundVersion = -1;
    private bool m_RoomEventsSubscribed;
    private bool m_RealityEventsSubscribed;

    private void OnEnable()
    {
        m_PanelRenderer = GetComponent<PanelRenderer>();
        if (m_PanelRenderer == null)
        {
            Debug.LogError("[UIShiftButton] PanelRenderer component not found!");
            return;
        }

        if (m_PanelRenderer.panelSettings == null)
        {
            Debug.LogError("[UIShiftButton] PanelRenderer has no PanelSettings assigned.");
            return;
        }

        if (m_PanelRenderer.visualTreeAsset == null)
        {
            Debug.LogError("[UIShiftButton] PanelRenderer has no Source Asset (UXML) assigned.");
            return;
        }

        m_ReloadCallback = HandlePanelReloaded;
        m_PanelRenderer.RegisterUIReloadCallback(m_ReloadCallback);

        TrySubscribeRuntimeEvents();
    }

    private void Start()
    {
        TrySubscribeRuntimeEvents();
    }

    private void OnDisable()
    {
        if (m_PanelRenderer != null && m_ReloadCallback != null)
        {
            m_PanelRenderer.UnregisterUIReloadCallback(m_ReloadCallback);
        }

        if (m_ShiftButton != null)
        {
            UnregisterCallbacks();
            m_ShiftButton = null;
        }

        UnsubscribeRuntimeEvents();
        m_LastBoundVersion = -1;
    }

    private void HandlePanelReloaded(PanelRenderer panelRenderer, VisualElement root, int version)
    {
        if (version == m_LastBoundVersion)
        {
            return;
        }

        m_LastBoundVersion = version;

        if (m_ShiftButton != null)
        {
            UnregisterCallbacks();
            m_ShiftButton = null;
        }

        if (root == null)
        {
            Debug.LogError("[UIShiftButton] Panel reloaded but root is null.");
            return;
        }

        m_ShiftButton = root.Q<VisualElement>("shift-button");
        if (m_ShiftButton == null)
        {
            Debug.LogError("[UIShiftButton] VisualElement 'shift-button' not found in active visual tree.");
            return;
        }

        RegisterCallbacks();
        RefreshButtonState();
    }

    private void RegisterCallbacks()
    {
        if (m_ShiftButton == null)
        {
            return;
        }

        m_ShiftButton.RegisterCallback<ClickEvent>(OnShiftButtonClicked);
    }

    private void UnregisterCallbacks()
    {
        if (m_ShiftButton == null)
        {
            return;
        }

        m_ShiftButton.UnregisterCallback<ClickEvent>(OnShiftButtonClicked);
    }

    private void OnShiftButtonClicked(ClickEvent evt)
    {
        if (RealityManager.Instance == null)
        {
            Debug.LogError("[UIShiftButton] RealityManager instance not found!");
            return;
        }

        var allowed = RealityManager.Instance.TryToggleReality();
        if (allowed)
        {
            Debug.Log("[UIShiftButton] Shift button clicked - toggling reality");
        }
        else
        {
            var roomId = RoomManager.Instance?.CurrentRoom?.id ?? "<unknown>";
            Debug.LogWarning($"[UIShiftButton] SHIFT unavailable in room '{roomId}'.");
        }
    }

    private void TrySubscribeRuntimeEvents()
    {
        if (!m_RoomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged += HandleRoomChanged;
            m_RoomEventsSubscribed = true;
        }

        if (!m_RealityEventsSubscribed && RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged += HandleRealityChanged;
            m_RealityEventsSubscribed = true;
        }

        RefreshButtonState();
    }

    private void UnsubscribeRuntimeEvents()
    {
        if (m_RoomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }

        if (m_RealityEventsSubscribed && RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged -= HandleRealityChanged;
        }

        m_RoomEventsSubscribed = false;
        m_RealityEventsSubscribed = false;
    }

    private void HandleRoomChanged(RoomData _)
    {
        RefreshButtonState();
    }

    private void HandleRealityChanged(RealityType _)
    {
        RefreshButtonState();
    }

    private void RefreshButtonState()
    {
        if (m_ShiftButton == null)
        {
            return;
        }

        var allowed = IsShiftAllowedInCurrentRoom();
        m_ShiftButton.SetEnabled(allowed);
        UpdateRealityClass();
    }

    private void UpdateRealityClass()
    {
        if (m_ShiftButton == null)
        {
            return;
        }

        var isOtherSide = RealityManager.Instance != null && RealityManager.Instance.CurrentReality == RealityType.OtherSide;

        m_ShiftButton.EnableInClassList("normal", !isOtherSide);
        m_ShiftButton.EnableInClassList("other-side", isOtherSide);
    }

    private bool IsShiftAllowedInCurrentRoom()
    {
        var roomId = RoomManager.Instance?.CurrentRoom?.id;
        if (string.IsNullOrEmpty(roomId))
        {
            return false;
        }

        foreach (var allowedRoom in shiftEnabledRoomIds)
        {
            if (!string.IsNullOrWhiteSpace(allowedRoom) &&
                roomId.Equals(allowedRoom, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
