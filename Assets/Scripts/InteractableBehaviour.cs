using UnityEngine;

/// <summary>
/// InteractableBehaviour handles mouse clicks on interactables.
/// When clicked (OnMouseDown), it delegates to InteractionManager.
/// 
/// Requirements:
/// - Must have a BoxCollider2D with IsTrigger = true
/// - Must have InteractableData assigned in the Inspector
/// - InteractionManager must exist in the scene
/// </summary>
public class InteractableBehaviour : MonoBehaviour
{
    [SerializeField] private InteractableData data;

    private void OnEnable()
    {
        // Debug check: is collider properly configured?
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            Debug.LogError($"[InteractableBehaviour] '{gameObject.name}' has no BoxCollider2D! OnMouseDown will not work.", gameObject);
        }
        else if (!collider.isTrigger)
        {
            Debug.LogError($"[InteractableBehaviour] '{gameObject.name}' BoxCollider2D IsTrigger is FALSE! Change it to TRUE.", gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (data == null)
        {
            Debug.LogError($"[InteractableBehaviour] '{gameObject.name}' has no InteractableData assigned!", gameObject);
            return;
        }

        if (InteractionManager.Instance == null)
        {
            Debug.LogError($"[InteractableBehaviour] InteractionManager instance not found. Click on '{data.id}' was ignored.");
            return;
        }

        Debug.Log($"[InteractableBehaviour] Clicked: {data.id}");
        InteractionManager.Instance.TryInteract(data.id);
    }
}
