using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InteractableBehaviour handles mouse clicks on interactables.
/// It also keeps visual/collider state in sync with room + reality rules.
/// </summary>
public class InteractableBehaviour : MonoBehaviour
{
    [SerializeField] private InteractableData data;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite otherSideSprite;
    [SerializeField] private bool useRealitySprites;

    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;
    private bool warnedMissingData;
    private Vector3 baseScale = Vector3.one;
    private bool isMainMenuButtonHovered;
    private bool colliderExternallyManaged;

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

    public void SetColliderExternallyManaged(bool externallyManaged)
    {
        colliderExternallyManaged = externallyManaged;
    }

    public void SetColliderEnabled(bool enabled)
    {
        if (hitCollider != null)
        {
            hitCollider.enabled = enabled;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
        baseScale = transform.localScale;
    }

    private void LateUpdate()
    {
        RefreshState();
    }

    private void Update()
    {
        if (!IsMouseOverThisObject())
        {
            ResetMainMenuButtonHover();
            return;
        }

        ApplyMainMenuButtonHover();

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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.UiClick);
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

        ApplyRealitySprite();

        var visible = true;
        if (RoomManager.Instance != null)
        {
            visible = RoomManager.Instance.IsInteractableVisible(data);
        }

        SetVisible(visible);
    }

    private void ApplyRealitySprite()
    {
        if (!useRealitySprites || spriteRenderer == null || data == null)
        {
            return;
        }

        if (data.id.Equals("shelf_interactive", StringComparison.OrdinalIgnoreCase)
            && GameManager.Instance != null
            && GameManager.Instance.HasFlag(GameFlagNames.LibraryShelfActivated))
        {
            if (normalSprite != null)
            {
                spriteRenderer.sprite = normalSprite;
            }

            return;
        }

        var targetSprite = RealityManager.Instance != null && RealityManager.Instance.CurrentReality == RealityType.OtherSide
            ? otherSideSprite
            : normalSprite;

        if (targetSprite != null)
        {
            spriteRenderer.sprite = targetSprite;
        }
    }

    private void SetVisible(bool visible)
    {
        if (data != null
            && data.id.Equals("denial_manifestation", StringComparison.OrdinalIgnoreCase))
        {
            var rejectionController = GetComponent<DenialManifestationController>();
            if (rejectionController == null)
            {
                rejectionController = GetComponentInChildren<DenialManifestationController>(true);
            }

            if (rejectionController != null && rejectionController.IsPlaying)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true;
                }

                if (hitCollider != null)
                {
                    hitCollider.enabled = false;
                }

                return;
            }
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }

        if (hitCollider != null && !colliderExternallyManaged)
        {
            hitCollider.enabled = visible;
        }

        if (!visible)
        {
            ResetMainMenuButtonHover();
        }
    }

    private void ApplyMainMenuButtonHover()
    {
        if (!IsMainMenuButton())
        {
            return;
        }

        isMainMenuButtonHovered = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.92f, 0.72f, 1f);
        }

        var pressed = Mouse.current != null && Mouse.current.leftButton.isPressed;
        transform.localScale = baseScale * (pressed ? 0.94f : 1.06f);
    }

    private void ResetMainMenuButtonHover()
    {
        if (!isMainMenuButtonHovered)
        {
            return;
        }

        isMainMenuButtonHovered = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        transform.localScale = baseScale;
    }

    private bool IsMainMenuButton()
    {
        if (data == null)
        {
            return false;
        }

        return data.id.Equals("main_menu_start", StringComparison.OrdinalIgnoreCase)
            || data.id.Equals("main_menu_settings", StringComparison.OrdinalIgnoreCase)
            || data.id.Equals("main_menu_exit", StringComparison.OrdinalIgnoreCase);
    }

    private void HandlePrimaryClick()
    {
        if (!TryResolveInteractionManager(out var interactionManager))
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.UiClick);
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
