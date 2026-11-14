using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InteractableBehaviour handles mouse clicks on interactables.
/// It also keeps visual/collider state in sync with room + reality rules.
/// </summary>
public class InteractableBehaviour : MonoBehaviour
{
    [SerializeField] private InteractableData data;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;
    private bool warnedMissingData;

    public void Configure(InteractableData config)
    {
        if (config == null)
        {
            return;
        }

        data = config;
        warnedMissingData = false;
    }

    public InteractableData GetData()
    {
        return data;
    }

    public bool HasData()
    {
        return data != null;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        RefreshState();
    }

    private void Update()
    {
        if (!IsMouseOverThisObject())
        {
            return;
        }

        var mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            HandlePrimaryClick();
        }
        else if (mouse.rightButton.wasPressedThisFrame)
        {
            HandleExamineClick();
        }
    }

    private bool IsMouseOverThisObject()
    {
        if (!TryResolveInteractionManager(out var interactionManager))
        {
            return false;
        }

        var mouse = Mouse.current;
        if (mouse == null)
        {
            return false;
        }

        var mousePos = mouse.position.ReadValue();
        var mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return false;
        }

        var ray = mainCamera.ScreenPointToRay(mousePos);
        var hits = Physics2D.RaycastAll(ray.origin, ray.direction);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleExamineClick()
    {
        if (!TryResolveInteractionManager(out var interactionManager))
        {
            return;
        }

        interactionManager.TryExamine(data);
        RefreshState();
    }

    private void RefreshState()
    {
        if (data == null)
        {
            SetVisible(false);
            return;
        }

        var visible = true;
        if (RoomManager.Instance != null)
        {
            visible = RoomManager.Instance.IsInteractableVisible(data);
        }

        SetVisible(visible);
    }

    private void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }

        if (hitCollider != null)
        {
            hitCollider.enabled = visible;
        }
    }

    private void HandlePrimaryClick()
    {
        if (!TryResolveInteractionManager(out var interactionManager))
        {
            return;
        }

        interactionManager.TryInteract(data, gameObject);
        RefreshState();
    }

    private bool TryResolveInteractionManager(out InteractionManager interactionManager)
    {
        interactionManager = InteractionManager.Instance;

        if (data == null)
        {
            if (!warnedMissingData)
            {
                warnedMissingData = true;
                Debug.LogError($"[InteractableBehaviour] '{gameObject.name}' has no InteractableData assigned.", gameObject);
            }

            return false;
        }

        if (interactionManager == null)
        {
            Debug.LogWarning($"[InteractableBehaviour] InteractionManager not found for '{data.id}'.");
            return false;
        }

        return true;
    }
}
