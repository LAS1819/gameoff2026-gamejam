using UnityEngine;

/// <summary>
/// InteractableBehaviour handles mouse clicks on interactables.
/// When clicked (OnMouseDown), it delegates to InteractionManager.
/// Requires a BoxCollider2D (IsTrigger = true) to detect clicks.
/// </summary>
public class InteractableBehaviour : MonoBehaviour
{
    [SerializeField] private InteractableData data;

    private void OnMouseDown()
    {
        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.TryInteract(data.id);
        }
        else
        {
            Debug.LogWarning($"[InteractableBehaviour] InteractionManager instance not found. Click on '{data.id}' was ignored.");
        }
    }
}
