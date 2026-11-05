using UnityEngine;

/// <summary>
/// UI Button handler for the SHIFT mechanic.
/// Connects Canvas button click to RealityManager.ToggleReality().
/// </summary>
public class UIShiftButton : MonoBehaviour
{
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
