using UnityEngine;

/// <summary>
/// UIManager handles all UI elements: text feedback, inventory display, status messages.
/// This is a stub for MVP. Later expansion: inventory UI, dialogue boxes, transitions.
/// For MVP, focus is on core interaction and flag progression, not visual feedback.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private string uiManagerName = "UIManager";
    [SerializeField] private GameObject feedbackRoot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

        if (feedbackRoot != null)
        {
            feedbackRoot.SetActive(true);
            // TODO: fade out after duration
        }
    }

    /// <summary>
    /// Hide feedback text.
    /// </summary>
    public void HideFeedback()
    {
        if (feedbackRoot != null)
        {
            feedbackRoot.SetActive(false);
        }
    }

    /// <summary>
    /// Update inventory UI display. Not implemented for MVP.
    /// </summary>
    public void RefreshInventoryUI()
    {
        Debug.Log($"[{uiManagerName}] RefreshInventoryUI() - stub method, not implemented for MVP");
    }
}
