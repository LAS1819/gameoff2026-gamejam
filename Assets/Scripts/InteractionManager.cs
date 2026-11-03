using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public event Action<string, InteractableData> InteractionResolved;

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

    public bool TryInteract(string interactableId)
    {
        if (string.IsNullOrEmpty(interactableId))
        {
            return false;
        }

        var room = RoomManager.Instance?.CurrentRoom;
        if (room == null)
        {
            return false;
        }

        var interactable = room.interactables.Find(item => item != null && item.id == interactableId);
        if (interactable == null)
        {
            Debug.LogWarning($"Interactable not found: {interactableId}");
            return false;
        }

        if (interactable.isPickedUp || interactable.isConsumed)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(interactable.requiredItemId) && !InventoryManager.Instance.HasItem(interactable.requiredItemId))
        {
            Debug.LogWarning($"Missing required item for {interactableId}: {interactable.requiredItemId}");
            return false;
        }

        if (interactable.type == InteractionType.Pickup)
        {
            interactable.isPickedUp = true;
            if (!string.IsNullOrEmpty(interactable.resultFlag))
            {
                GameManager.Instance.SetFlag(interactable.resultFlag, true);
            }

            if (!string.IsNullOrEmpty(interactable.itemId))
            {
                InventoryManager.Instance.AddItem(new InventoryItemData
                {
                    id = interactable.itemId,
                    displayName = interactable.itemName,
                    description = interactable.itemDescription,
                    isKeyItem = true
                });
            }

            if (interactable.consumeAfterUse)
            {
                interactable.isConsumed = true;
            }

            RoomManager.Instance.RefreshInteractables();
            InteractionResolved?.Invoke(interactableId, interactable);
            return true;
        }

        if (interactable.type == InteractionType.Use && !string.IsNullOrEmpty(interactable.resultFlag))
        {
            interactable.isConsumed = true;
            GameManager.Instance.SetFlag(interactable.resultFlag, true);
            RoomManager.Instance.RefreshInteractables();
            InteractionResolved?.Invoke(interactableId, interactable);
            return true;
        }

        if (interactable.type == InteractionType.Toggle && !string.IsNullOrEmpty(interactable.resultFlag))
        {
            GameManager.Instance.SetFlag(interactable.resultFlag, !GameManager.Instance.HasFlag(interactable.resultFlag));
            RoomManager.Instance.RefreshInteractables();
            InteractionResolved?.Invoke(interactableId, interactable);
            return true;
        }

        if (interactable.type == InteractionType.EnterRoom && !string.IsNullOrEmpty(interactable.linkedRoomId))
        {
            RoomManager.Instance.LoadRoom(interactable.linkedRoomId);
            InteractionResolved?.Invoke(interactableId, interactable);
            return true;
        }

        return false;
    }
}
