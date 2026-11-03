using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager handles all UI elements: text feedback, inventory display, status messages.
/// This is a stub for MVP. Later expansion: inventory UI, dialogue boxes, transitions.
/// For MVP, focus is on core interaction and flag progression, not visual feedback.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private string uiManagerName = "UIManager";
    [SerializeField] private Text feedbackText;  // Optional: drag a Text canvas element here

    private void Start()
    {
        Debug.Log($"[{uiManagerName}] Initialized (stub for MVP)");
    }

    /// <summary>
    /// Display temporary feedback text. Later: fade in/out with duration.
    /// </summary>
    public void ShowFeedback(string message, float duration = 2f)
    {
        Debug.Log($"[{uiManagerName}] ShowFeedback: {message}");
        
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
            // TODO: fade out after duration
        }
    }

    /// <summary>
    /// Hide feedback text.
    /// </summary>
    public void HideFeedback()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update inventory UI display. Not implemented for MVP.
    /// </summary>
    public void RefreshInventoryUI()
    {
        Debug.Log($"[{uiManagerName}] RefreshInventoryUI() - stub method, not implemented for MVP");
    }
}
