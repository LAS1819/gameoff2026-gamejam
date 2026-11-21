using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    public Transform roomsParent;

    public void CreateRoom(RoomData roomData)
    {
        if (roomData == null)
        {
            return;
        }

        if (roomsParent == null)
        {
            var existingParent = GameObject.Find("Rooms");
            roomsParent = existingParent != null ? existingParent.transform : null;
        }

        if (roomsParent == null)
        {
            var roomsRoot = new GameObject("Rooms");
            roomsParent = roomsRoot.transform;
        }

        var roomGo = new GameObject(roomData.id);
        roomGo.transform.SetParent(roomsParent, false);
        roomGo.transform.position = Vector3.zero;

        foreach (var interactable in roomData.interactables)
        {
            if (interactable == null)
            {
                continue;
            }

            CreateInteractable(roomGo.transform, interactable);
        }
    }

    public GameObject CreateInteractable(Transform roomRoot, InteractableData data)
    {
        if (data == null)
        {
            return null;
        }

        var go = new GameObject(data.id);
        go.transform.SetParent(roomRoot, false);
        go.transform.localPosition = Vector3.zero;

        go.AddComponent<BoxCollider2D>().isTrigger = true;
        go.AddComponent<SpriteRenderer>();

        var behaviour = go.AddComponent<InteractableBehaviour>();
        behaviour.Configure(data);

        return go;
    }
}
