using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HUDMemoryDiscoveryView
{
    private readonly MonoBehaviour owner;
    private readonly Sprite[] concepts;
    private VisualElement overlay;
    private VisualElement image;
    private Label title;
    private Label body;
    private Button continueButton;

    public HUDMemoryDiscoveryView(
        MonoBehaviour owner,
        VisualElement root,
        Sprite memory01,
        Sprite memory02,
        Sprite memory03,
        Sprite memory04,
        Sprite memory05,
        float discoveryDuration)
    {
        this.owner = owner;
        concepts = new[] { memory01, memory02, memory03, memory04, memory05 };
        _ = discoveryDuration;
        Build(root);
    }

    public void Update()
    {
        if (overlay == null || !overlay.ClassListContains("visible"))
        {
            return;
        }

        var keyboard = Keyboard.current;
        if (keyboard != null
            && (keyboard.escapeKey.wasPressedThisFrame
                || keyboard.enterKey.wasPressedThisFrame
                || keyboard.spaceKey.wasPressedThisFrame))
        {
            Close();
        }
    }

    public void Show(MemoryDefinition memory)
    {
        if (memory == null)
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.MemoryDiscovery);
        }

        title.text = memory.title;
        body.text = memory.body;
        var concept = ResolveConcept(memory.id);
        image.style.backgroundImage = concept != null
            ? new StyleBackground(concept)
            : StyleKeyword.Null;
        overlay.style.opacity = 1f;
        overlay.style.display = DisplayStyle.Flex;
        overlay.AddToClassList("visible");
        continueButton.focusable = true;
        continueButton.Focus();
    }

    public void Close()
    {
        if (overlay != null)
        {
            overlay.style.opacity = 0f;
            overlay.style.display = DisplayStyle.None;
            overlay.RemoveFromClassList("visible");
        }
    }

    private Sprite ResolveConcept(string memoryId)
    {
        var index = memoryId switch
        {
            GameFlagNames.Memory_01 => 0,
            GameFlagNames.Memory_02 => 1,
            GameFlagNames.Memory_03 => 2,
            GameFlagNames.Memory_04 => 3,
            GameFlagNames.Memory_05 => 4,
            _ => -1
        };
        return index >= 0 ? concepts[index] : null;
    }

    private void Build(VisualElement root)
    {
        if (root == null)
        {
            return;
        }

        overlay = root.Q<VisualElement>("memory-discovery-overlay");
        if (overlay == null)
        {
            overlay = new VisualElement { name = "memory-discovery-overlay" };
            overlay.AddToClassList("memory-discovery-overlay");
            root.Add(overlay);
        }

        overlay.pickingMode = PickingMode.Position;
        overlay.style.display = DisplayStyle.None;

        var backdrop = overlay.Q<VisualElement>("memory-discovery-backdrop");
        if (backdrop == null)
        {
            backdrop = new VisualElement { name = "memory-discovery-backdrop" };
            backdrop.AddToClassList("memory-discovery-backdrop");
            backdrop.pickingMode = PickingMode.Ignore;
            overlay.Add(backdrop);
        }

        var panel = overlay.Q<VisualElement>("memory-discovery-panel");
        if (panel == null)
        {
            panel = new VisualElement { name = "memory-discovery-panel" };
            panel.AddToClassList("memory-discovery-panel");
            panel.pickingMode = PickingMode.Position;
            overlay.Add(panel);
        }

        image = panel.Q<VisualElement>("memory-discovery-image");
        if (image == null)
        {
            image = new VisualElement { name = "memory-discovery-image" };
            image.AddToClassList("memory-discovery-image");
            panel.Add(image);
        }

        title = panel.Q<Label>("memory-discovery-title");
        if (title == null)
        {
            title = new Label { name = "memory-discovery-title" };
            title.AddToClassList("memory-discovery-title");
            panel.Add(title);
        }

        body = panel.Q<Label>("memory-discovery-body");
        if (body == null)
        {
            body = new Label { name = "memory-discovery-body" };
            body.AddToClassList("memory-discovery-body");
            panel.Add(body);
        }

        continueButton = panel.Q<Button>("memory-discovery-close");
        if (continueButton == null)
        {
            continueButton = new Button
            {
                name = "memory-discovery-close",
                text = "Continue"
            };
            continueButton.AddToClassList("memory-discovery-close");
            continueButton.pickingMode = PickingMode.Position;
            panel.Add(continueButton);
        }

        continueButton.UnregisterCallback<ClickEvent>(OnContinueClicked);
        continueButton.RegisterCallback<ClickEvent>(OnContinueClicked);
    }

    private void OnContinueClicked(ClickEvent _)
    {
        Close();
    }

}
