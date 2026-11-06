using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI Button handler for the SHIFT mechanic.
/// Connects Canvas button click to RealityManager.ToggleReality().
/// </summary>

[RequireComponent(typeof(PanelRenderer))]
public class UIShiftButton : MonoBehaviour
{
    private PanelRenderer m_PanelRenderer;
    private Button m_ShiftButton;

    void Awake()
    {
        m_PanelRenderer = GetComponent<PanelRenderer>();
        var root = m_PanelRenderer.visualTreeAsset.CloneTree();
        m_ShiftButton = root.Q<Button>("shift-button");

        // Debug
        if (m_PanelRenderer == null)
        {
            Debug.LogError("[UIShiftButton] PanelRenderer component not found!");
        }
        else
        {
            Debug.Log("[UIShiftButton] PanelRenderer component found");
        }
        if (m_ShiftButton == null)
        {
            Debug.LogError("[UIShiftButton] Button component not found!");
        }
        else
        {
            Debug.Log("[UIShiftButton] Button component found");
            m_ShiftButton.clicked += OnShiftButtonClicked;
        }
    }

    public void OnShiftButtonClicked()
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
