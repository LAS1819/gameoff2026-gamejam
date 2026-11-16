using UnityEngine;

public class ShelfSecretDoorController : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Vector3 openLocalOffset = new(2.25f, 0f, 0f);
    [SerializeField] private float moveSpeed = 1.4f;
    [SerializeField] private float arrivedThreshold = 0.02f;
    [SerializeField] private GameObject basementDoorObject;
    [SerializeField] private GameObject codePanelObject;

    private Vector3 closedLocalPosition;
    private bool desiredOpenState;
    private bool revealObjectsVisible;

    private void Awake()
    {
        if (doorTransform == null)
        {
            doorTransform = transform;
        }

        closedLocalPosition = doorTransform.localPosition;
        ResolveTargetsIfNeeded();
    }

    private void OnEnable()
    {
        ResolveTargetsIfNeeded();
        SyncFromFlags();
    }

    private void Update()
    {
        ResolveTargetsIfNeeded();
        SyncFromFlags();
        MoveDoor();
        UpdateRevealObjects();
    }

    public void SetOpen(bool open)
    {
        desiredOpenState = open;
    }

    private void SyncFromFlags()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        SetOpen(GameManager.Instance.HasFlag(GameFlagNames.LibrarySecretDoorOpen));
    }

    private void MoveDoor()
    {
        if (doorTransform == null)
        {
            return;
        }

        var targetPosition = desiredOpenState
            ? closedLocalPosition + openLocalOffset
            : closedLocalPosition;

        doorTransform.localPosition = Vector3.MoveTowards(
            doorTransform.localPosition,
            targetPosition,
            moveSpeed * Time.deltaTime);

        if (Vector3.Distance(doorTransform.localPosition, targetPosition) <= arrivedThreshold)
        {
            doorTransform.localPosition = targetPosition;
        }
    }

    private void UpdateRevealObjects()
    {
        if (doorTransform == null)
        {
            return;
        }

        var shouldReveal = desiredOpenState
            && Vector3.Distance(doorTransform.localPosition, closedLocalPosition + openLocalOffset) <= arrivedThreshold;

        if (revealObjectsVisible == shouldReveal)
        {
            return;
        }

        revealObjectsVisible = shouldReveal;

        if (basementDoorObject != null)
        {
            basementDoorObject.SetActive(shouldReveal);
        }

        if (codePanelObject != null)
        {
            codePanelObject.SetActive(shouldReveal);
        }
    }

    private void ResolveTargetsIfNeeded()
    {
        if (basementDoorObject == null)
        {
            basementDoorObject = FindNamedObject("Basement Door", "basement_door", "BasementDoor");
        }

        if (codePanelObject == null)
        {
            codePanelObject = FindNamedObject("Code Panel", "code_panel", "CodePanel");
        }
    }

    private static GameObject FindNamedObject(params string[] names)
    {
        foreach (var name in names)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            var found = GameObject.Find(name);
            if (found != null)
            {
                return found;
            }

            var compactName = name.Replace(" ", string.Empty).Replace("_", string.Empty);
            found = GameObject.Find(compactName);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}
