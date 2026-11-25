using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HUDStoryOverlayView
{
    private readonly HUDController hud;
    private VisualElement root;
    private VisualElement overlay;
    private VisualElement panel;
    private Label eyebrow;
    private Label title;
    private Label body;
    private Label memoryCount;
    private Label credits;
    private VisualElement actions;
    private Button close;
    private Button restart;
    private StoryOverlayMode mode = StoryOverlayMode.Letter;
    private int introPage;
    private string introPageOneTitle;
    private string introPageOneBody;
    private string introPageTwoTitle;
    private string introPageTwoBody;

    public bool IsVisible
    {
        get
        {
            return overlay != null && overlay.ClassListContains("visible");
        }
    }

    public HUDStoryOverlayView(HUDController hud, VisualElement root)
    {
        this.hud = hud;
        this.root = root;
        Build();
        Bind();
    }

    public void Update()
    {
        if (!IsVisible)
        {
            return;
        }

        var keyboard = Keyboard.current;
        if (keyboard != null
            && (keyboard.escapeKey.wasPressedThisFrame
                || keyboard.enterKey.wasPressedThisFrame
                || keyboard.spaceKey.wasPressedThisFrame))
        {
            HandlePrimaryAction();
            return;
        }

        var mouse = Mouse.current;
        if (mouse != null
            && mouse.leftButton.wasPressedThisFrame
            && TryGetHudPoint(mouse.position.ReadValue(), out var point))
        {
            if ((mode == StoryOverlayMode.Letter || mode == StoryOverlayMode.Intro)
                && close != null
                && close.worldBound.Contains(point))
            {
                Close();
            }
            else if (mode == StoryOverlayMode.Ending
                && restart != null
                && restart.worldBound.Contains(point))
            {
                Restart();
            }
        }
    }

    public void ShowLetter(string overlayTitle, string overlayBody, string hint)
    {
        mode = StoryOverlayMode.Letter;
        panel.EnableInClassList("story-overlay-panel-letter", true);
        eyebrow.text = "A LETTER";
        title.text = overlayTitle;
        title.style.fontSize = 18f;
        title.style.whiteSpace = WhiteSpace.Normal;
        body.text = overlayBody;
        memoryCount.text = string.Empty;
        credits.text = string.Empty;
        close.text = hint;
        close.style.display = DisplayStyle.Flex;
        close.SetEnabled(true);
        close.focusable = true;
        restart.style.display = DisplayStyle.None;
        restart.SetEnabled(false);
        Show();
        ApplyLayout(mode);
        close.Focus();
    }

    public void ShowIntro(string introTitle, string introBody, string hint)
    {
        mode = StoryOverlayMode.Intro;
        introPage = 0;
        introPageOneTitle = introTitle;
        introPageOneBody = introBody;
        introPageTwoTitle = "The invitation";
        introPageTwoBody = "It asked me to meet him inside the building marked HORIZON.\n\nI came here to find answers. I have not yet understood that I came to say goodbye.";
        panel.EnableInClassList("story-overlay-panel-letter", true);
        eyebrow.text = "INTRODUCTION 1/2";
        title.text = introPageOneTitle;
        title.style.fontSize = 16f;
        title.style.whiteSpace = WhiteSpace.Normal;
        body.text = introPageOneBody;
        memoryCount.text = string.Empty;
        credits.text = string.Empty;
        close.text = "Next";
        close.style.display = DisplayStyle.Flex;
        close.SetEnabled(true);
        close.focusable = true;
        restart.style.display = DisplayStyle.None;
        restart.SetEnabled(false);
        Show();
        ApplyLayout(StoryOverlayMode.Intro);
        close.Focus();
    }

    public void ShowEnding(string endingTitle, string farewell, string creditsText, int recoveredCount)
    {
        mode = StoryOverlayMode.Ending;
        panel.EnableInClassList("story-overlay-panel-letter", false);
        eyebrow.text = "ENDING";
        title.text = endingTitle;
        title.style.fontSize = 16f;
        title.style.whiteSpace = WhiteSpace.Normal;
        body.text = farewell;
        memoryCount.text = $"Recovered Memories: {Mathf.Clamp(recoveredCount, 0, 5)} / 5";
        credits.text = creditsText;
        close.style.display = DisplayStyle.None;
        close.SetEnabled(false);
        restart.text = "Restart game";
        restart.style.display = DisplayStyle.Flex;
        restart.SetEnabled(true);
        restart.focusable = true;
        Show();
        ApplyLayout(mode);
        restart.Focus();
    }

    public void Hide()
    {
        if (overlay != null)
        {
            overlay.RemoveFromClassList("visible");
            overlay.style.display = DisplayStyle.None;
        }
        hud.SetGameplayControlsVisible(true);
    }

    private void Build()
    {
        if (root == null)
        {
            return;
        }

        overlay = root.Q<VisualElement>("hud-modal-layer");
        if (overlay == null)
        {
            overlay = new VisualElement { name = "hud-modal-layer" };
            root.Add(overlay);
        }
        overlay.RemoveFromClassList("visible");
        overlay.style.display = DisplayStyle.None;

        var backdrop = overlay.Q<VisualElement>("story-overlay-backdrop");
        if (backdrop == null)
        {
            backdrop = new VisualElement { name = "story-overlay-backdrop" };
            backdrop.AddToClassList("story-overlay-backdrop");
            backdrop.pickingMode = PickingMode.Ignore;
            overlay.Add(backdrop);
        }

        panel = overlay.Q<VisualElement>("story-overlay-panel");
        if (panel == null)
        {
            panel = new VisualElement { name = "story-overlay-panel" };
            panel.AddToClassList("story-overlay-panel");
            overlay.Add(panel);
        }

        eyebrow = GetOrCreateLabel(panel, "story-overlay-eyebrow", "story-overlay-eyebrow");
        title = GetOrCreateLabel(panel, "story-overlay-title", "story-overlay-title");
        body = GetOrCreateLabel(panel, "story-overlay-body", "story-overlay-body");
        memoryCount = GetOrCreateLabel(panel, "story-overlay-memory-count", "story-overlay-memory-count");
        credits = GetOrCreateLabel(panel, "story-overlay-credits", "story-overlay-credits");

        actions = panel.Q<VisualElement>("story-overlay-actions");
        if (actions == null)
        {
            actions = new VisualElement { name = "story-overlay-actions" };
            actions.AddToClassList("story-overlay-actions");
            actions.style.position = Position.Absolute;
            actions.style.left = 0f;
            actions.style.right = 0f;
            actions.style.bottom = 18f;
            actions.style.height = 42f;
            panel.Add(actions);
        }

        close = GetOrCreateButton(actions, "story-overlay-close", "Continue");
        close.AddToClassList("story-overlay-close");
        close.style.display = DisplayStyle.None;
        close.SetEnabled(false);
        close.focusable = false;
        restart = GetOrCreateButton(actions, "story-overlay-restart", "Restart game");
        restart.AddToClassList("story-overlay-restart");
        restart.style.display = DisplayStyle.None;
        restart.SetEnabled(false);
        restart.focusable = false;
    }

    private static Label GetOrCreateLabel(VisualElement panel, string name, string className)
    {
        var element = panel.Q<Label>(name);
        if (element == null)
        {
            element = new Label { name = name };
            element.AddToClassList(className);
            panel.Add(element);
        }
        return element;
    }

    private static Button GetOrCreateButton(VisualElement panel, string name, string text)
    {
        var element = panel.Q<Button>(name);
        if (element == null)
        {
            element = new Button { name = name, text = text };
            element.pickingMode = PickingMode.Position;
            element.style.position = Position.Absolute;
            element.style.bottom = 0f;
            element.style.left = 122f;
            element.style.width = 176f;
            element.style.height = 42f;
            panel.Add(element);
        }
        return element;
    }

    private void Bind()
    {
        close.UnregisterCallback<PointerDownEvent>(OnClosePointerDown);
        close.RegisterCallback<PointerDownEvent>(OnClosePointerDown);
        restart.UnregisterCallback<PointerDownEvent>(OnRestartPointerDown);
        restart.RegisterCallback<PointerDownEvent>(OnRestartPointerDown);
    }

    private void OnClosePointerDown(PointerDownEvent _) => Close();
    private void OnRestartPointerDown(PointerDownEvent _) => Restart();

    private void HandlePrimaryAction()
    {
        if (mode == StoryOverlayMode.Letter || mode == StoryOverlayMode.Intro)
        {
            Close();
        }
        else
        {
            Restart();
        }
    }

    private void Close()
    {
        if (mode == StoryOverlayMode.Intro)
        {
            if (introPage == 0)
            {
                introPage = 1;
                eyebrow.text = "INTRODUCTION 2/2";
                title.text = introPageTwoTitle;
                body.text = introPageTwoBody;
                close.text = "Continue";
                close.Focus();
                return;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetFlag(GameFlagNames.MainMenuEntryUnlocked, true);
            }
            if (RoomManager.Instance != null)
            {
                RoomManager.Instance.RefreshInteractables();
            }
            Hide();
            return;
        }

        if (mode != StoryOverlayMode.Letter)
        {
            Hide();
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.ForestEndingReached, true);
        }
        var count = GameManager.Instance != null
            ? GameManager.Instance.CountRecoveredMemories()
            : 0;
        var ending = ResolveEnding(count);
        ShowEnding(ending.title, ending.farewell, "Game design and development", count);
    }

    private static (string title, string farewell) ResolveEnding(int count)
    {
        if (count <= 2)
        {
            return ("Bad Ending\nDenial", "You are still not ready to leave the lie behind. The farewell remains unfinished.");
        }
        if (count <= 4)
        {
            return ("Neutral Ending\nEscape", "You understood part of the truth, but there is still one word that could not be spoken.");
        }
        return ("True Ending\nReunion", "You accept his death and your own. At last, you can stop searching for answers and say goodbye.");
    }

    private void Restart()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        Hide();
        GameManager.Instance.InitializeMVP();
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.LoadRoom(GameManager.MainMenuRoomId);
        }
        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.SetReality(RealityType.Normal);
        }
    }

    private void Show()
    {
        hud.SetGameplayControlsVisible(false);
        ExpandToRoot();
        overlay.style.display = DisplayStyle.Flex;
        overlay.AddToClassList("visible");
    }

    private void ExpandToRoot()
    {
        var width = root != null ? root.worldBound.width : 640f;
        var height = root != null ? root.worldBound.height : 360f;
        if (width <= 0f) width = 640f;
        if (height <= 0f) height = 360f;

        overlay.style.position = Position.Absolute;
        overlay.style.left = 0f;
        overlay.style.top = 0f;
        overlay.style.width = width;
        overlay.style.height = height;
        overlay.style.alignItems = Align.Center;
        overlay.style.justifyContent = Justify.Center;

        var panelWidth = Mathf.Clamp(width * 0.58f, 420f, 620f);
        var panelHeight = Mathf.Clamp(height - 24f, 220f, 320f);
        panel.style.position = Position.Relative;
        panel.style.left = StyleKeyword.Auto;
        panel.style.top = StyleKeyword.Auto;
        panel.style.width = panelWidth;
        panel.style.height = panelHeight;
        panel.style.minHeight = 220f;
        panel.style.maxHeight = 320f;
        panel.style.overflow = Overflow.Visible;
    }

    private void ApplyLayout(StoryOverlayMode layoutMode)
    {
        if (layoutMode == StoryOverlayMode.Letter || layoutMode == StoryOverlayMode.Intro)
        {
            title.style.position = Position.Relative;
            body.style.position = Position.Relative;
            memoryCount.style.position = Position.Relative;
            credits.style.position = Position.Relative;
            title.style.marginBottom = 4f;
            body.style.marginTop = 6f;
            body.style.marginBottom = 8f;
            body.style.flexShrink = 1f;
            body.style.overflow = Overflow.Visible;
            actions.style.position = Position.Relative;
            actions.style.bottom = StyleKeyword.Auto;
            close.style.position = StyleKeyword.Auto;
            close.style.left = StyleKeyword.Auto;
            close.style.bottom = StyleKeyword.Auto;
            return;
        }

        title.style.position = Position.Relative;
        title.style.left = StyleKeyword.Auto;
        title.style.top = StyleKeyword.Auto;
        title.style.marginBottom = 4f;
        body.style.position = Position.Relative;
        body.style.left = StyleKeyword.Auto;
        body.style.top = StyleKeyword.Auto;
        body.style.marginTop = 4f;
        body.style.marginBottom = 6f;
        memoryCount.style.position = Position.Relative;
        memoryCount.style.left = StyleKeyword.Auto;
        memoryCount.style.top = StyleKeyword.Auto;
        memoryCount.style.marginBottom = 4f;
        credits.style.position = Position.Relative;
        credits.style.left = StyleKeyword.Auto;
        credits.style.top = StyleKeyword.Auto;
        credits.style.marginBottom = 8f;
        actions.style.position = Position.Relative;
        actions.style.bottom = StyleKeyword.Auto;
        restart.style.position = StyleKeyword.Auto;
        restart.style.left = StyleKeyword.Auto;
        restart.style.bottom = StyleKeyword.Auto;
    }

    private bool TryGetHudPoint(Vector2 screenPoint, out Vector2 point)
    {
        var width = root != null ? root.worldBound.width : 640f;
        var height = root != null ? root.worldBound.height : 360f;
        if (width <= 0f) width = 640f;
        if (height <= 0f) height = 360f;

        point = new Vector2(
            screenPoint.x * width / Mathf.Max(Screen.width, 1),
            (Screen.height - screenPoint.y) * height / Mathf.Max(Screen.height, 1));
        return true;
    }
}
