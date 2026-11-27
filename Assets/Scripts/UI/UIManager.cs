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

    public void RefreshLanternState()
    {
        TryResolveHudController();
        if (hudController != null)
        {
            hudController.RefreshLanternState();
        }
    }

    public void ShowStoryOverlay(string title, string body, string hint = "Continue", StoryOverlayMode mode = StoryOverlayMode.Letter)
    {
        TryResolveHudController();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.StoryOpen);
        }
        if (hudController != null)
        {
            hudController.ShowStoryOverlay(title, body, hint, mode);
        }
    }

    public void ShowIntro(string title, string body, string hint = "Continue")
    {
        TryResolveHudController();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.StoryOpen);
        }
        if (hudController != null)
        {
            hudController.ShowIntro(title, body, hint);
        }
    }

    public void ShowMemoryDiscovery(MemoryDefinition memory)
    {
        TryResolveHudController();
        if (hudController != null)
        {
            hudController.ShowMemoryDiscovery(memory);
        }
    }

    public void ShowEndingOverlay(string title, string farewell, int memoryCount)
    {
        TryResolveHudController();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.StoryOpen);
        }
        if (hudController != null)
        {
            hudController.ShowEndingOverlay(title, farewell, memoryCount);
        }
    }

    public void HideStoryOverlay()
    {
        TryResolveHudController();
        if (hudController != null)
        {
            hudController.HideStoryOverlay();
        }
    }

    public bool IsStoryOverlayVisible()
    {
        TryResolveHudController();
        return hudController != null && hudController.IsStoryOverlayVisible();
    }

    private void TryResolveHudController()
    {
        if (hudController == null)
        {
            hudController = FindAnyObjectByType<HUDController>();
        }
    }
}
