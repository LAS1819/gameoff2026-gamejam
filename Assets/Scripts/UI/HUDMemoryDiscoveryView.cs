using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDMemoryDiscoveryView
{
    private readonly MonoBehaviour owner;
    private readonly Sprite[] concepts;
    private VisualElement overlay;
    private VisualElement image;
    private Label title;
    private Label body;
    private Coroutine routine;

    public HUDMemoryDiscoveryView(
        MonoBehaviour owner,
        VisualElement root,
        Sprite memory01,
        Sprite memory02,
        Sprite memory03,
        Sprite memory04,
        Sprite memory05)
    {
        this.owner = owner;
        concepts = new[] { memory01, memory02, memory03, memory04, memory05 };
        Build(root);
    }

    public void Show(MemoryDefinition memory)
    {
        if (memory == null)
        {
            return;
        }

        title.text = memory.title;
        body.text = memory.body;
        var concept = ResolveConcept(memory.id);
        image.style.backgroundImage = concept != null
            ? new StyleBackground(concept)
            : StyleKeyword.Null;

        if (routine != null)
        {
            owner.StopCoroutine(routine);
        }
        routine = owner.StartCoroutine(Play(memory.duration));
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

        overlay.pickingMode = PickingMode.Ignore;
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
            panel.pickingMode = PickingMode.Ignore;
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
    }

    private IEnumerator Play(float duration)
    {
        overlay.style.display = DisplayStyle.Flex;
        yield return Fade(0.3f, 0f, 1f);
        overlay.style.opacity = 1f;
        yield return new WaitForSecondsRealtime(Mathf.Max(0.5f, duration - 0.9f));
        yield return Fade(0.6f, 1f, 0f);
        overlay.style.opacity = 0f;
        overlay.style.display = DisplayStyle.None;
        routine = null;
    }

    private IEnumerator Fade(float length, float from, float to)
    {
        for (var elapsed = 0f; elapsed < length; elapsed += Time.unscaledDeltaTime)
        {
            var amount = Mathf.Clamp01(elapsed / length);
            overlay.style.opacity = Mathf.Lerp(from, to, amount);
            yield return null;
        }
        overlay.style.opacity = to;
    }
}
