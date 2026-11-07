using UnityEngine;

/// <summary>
/// UIManager is a lightweight façade used by gameplay systems to interact with runtime HUD.
/// Runtime rendering and bindings are handled by HUDController.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private string uiManagerName = "UIManager";
    [SerializeField] private HUDController hudController;

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
        TryResolveHudController();
        Debug.Log($"[{uiManagerName}] Initialized");
    }

    /// <summary>
    /// Display temporary feedback text through HUDController.
    /// </summary>
    public void ShowFeedback(string message, float duration = 2f)
    {
        TryResolveHudController();
        if (hudController != null)
        {
            hudController.ShowFeedback(message, duration);
        }
        else
        {
            Debug.Log($"[{uiManagerName}] ShowFeedback fallback: {message}");
        }
    }

    /// <summary>
    /// Hide feedback text.
    /// </summary>
    public void HideFeedback()
    {
        ShowFeedback(string.Empty, 0f);
    }

    /// <summary>
    /// Refresh inventory UI state.
    /// </summary>
    public void RefreshInventoryUI()
    {
        TryResolveHudController();
        if (hudController != null)
        {
            hudController.RefreshInventory();
        }
    }

    private void TryResolveHudController()
    {
        if (hudController == null)
        {
            hudController = FindAnyObjectByType<HUDController>();
        }
    }
}
