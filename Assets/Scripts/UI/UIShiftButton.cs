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

    private void OnEnable()
    {
        // Get the PanelRenderer component
        m_PanelRenderer = GetComponent<PanelRenderer>();

        if (m_PanelRenderer == null)
        {
            Debug.LogError("[UIShiftButton] PanelRenderer component not found!");
            return;
        }

        Debug.Log("[UIShiftButton] PanelRenderer found");

        // Access the root visual element of the panel
        var rootElement = m_PanelRenderer.rootVisualElement;

        if (rootElement == null)
        {
            Debug.LogError("[UIShiftButton] Root visual element is null!");
            return;
        }

        Debug.Log("[UIShiftButton] Root visual element found");

        // Query for the button by name in the visual tree
        m_ShiftButton = rootElement.Q<Button>("shift-button");

        if (m_ShiftButton == null)
        {
            Debug.LogError("[UIShiftButton] Button 'shift-button' not found in UXML!");
            Debug.Log("[UIShiftButton] Available elements: " + string.Join(", ", rootElement.Query().Build().ToList().Select(e => e.name)));
            return;
        }

        Debug.Log("[UIShiftButton] Shift button found - registering click handler");

        // Register the click callback
        m_ShiftButton.clicked += OnShiftButtonClicked;
    }

    private void OnDisable()
    {
        // Unregister the click callback to avoid memory leaks
        if (m_ShiftButton != null)
        {
            m_ShiftButton.clicked -= OnShiftButtonClicked;
        }
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

