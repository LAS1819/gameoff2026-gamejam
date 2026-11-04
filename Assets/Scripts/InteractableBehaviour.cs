using UnityEngine;

/// <summary>
/// InteractableBehaviour handles mouse clicks on interactables.
/// It also keeps visual/collider state in sync with room + reality rules.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class InteractableBehaviour : MonoBehaviour
{
    [SerializeField] private InteractableData data;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;
    private bool warnedMissingData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        RefreshState();
    }

    private void OnMouseDown()
    {
        if (data == null)
        {
            if (!warnedMissingData)
            {
                warnedMissingData = true;
                Debug.LogError($"[InteractableBehaviour] '{gameObject.name}' has no InteractableData assigned.", gameObject);
            }

            return;
        }

        if (InteractionManager.Instance == null)
        {
            Debug.LogWarning($"[InteractableBehaviour] InteractionManager not found for '{data.id}'.");
            return;
        }

        InteractionManager.Instance.TryInteract(data);
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
}
