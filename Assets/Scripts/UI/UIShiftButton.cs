using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI Button handler for the SHIFT mechanic using UI Toolkit.
/// Connects Panel Renderer button click to RealityManager.ToggleReality().
/// </summary>
[RequireComponent(typeof(PanelRenderer))]
public class UIShiftButton : MonoBehaviour
{
    private PanelRenderer m_PanelRenderer;
    private Button m_ShiftButton;
    private PanelRenderer.VersionedUIReloadCallback m_ReloadCallback;
    private int m_LastBoundVersion = -1;

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
        Debug.Log("[UIShiftButton] Registered panel reload callback.");
    }

    private void OnDisable()
    {
        if (m_PanelRenderer != null && m_ReloadCallback != null)
        {
            m_PanelRenderer.UnregisterUIReloadCallback(m_ReloadCallback);
        }

        if (m_ShiftButton != null)
        {
            m_ShiftButton.clicked -= OnShiftButtonClicked;
            m_ShiftButton = null;
        }

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
            m_ShiftButton.clicked -= OnShiftButtonClicked;
            m_ShiftButton = null;
        }

        if (root == null)
        {
            Debug.LogError("[UIShiftButton] Panel reloaded but root is null.");
            return;
        }

        m_ShiftButton = root.Q<Button>("shift-button");
        if (m_ShiftButton == null)
        {
            Debug.LogError("[UIShiftButton] Button 'shift-button' not found in active visual tree.");
            return;
        }

        m_ShiftButton.clicked += OnShiftButtonClicked;
        Debug.Log("[UIShiftButton] Bound click handler to 'shift-button'.");
    }

    private void OnShiftButtonClicked()
    {
        Debug.Log("[UIShiftButton] Shift button clicked - toggling reality");

        if (RealityManager.Instance == null)
        {
            Debug.LogError("[UIShiftButton] RealityManager instance not found!");
            return;
        }

        RealityManager.Instance.ToggleReality();
    }
}
