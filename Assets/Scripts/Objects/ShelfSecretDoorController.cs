using UnityEngine;

public class ShelfSecretDoorController : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Vector3 openLocalOffset = new(10.25f, 0f, 0f);
    [SerializeField] private float moveSpeed = 1.4f;
    [SerializeField] private float arrivedThreshold = 0.02f;
    [SerializeField] private GameObject basementDoorObject;
    [SerializeField] private GameObject codePanelObject;

    private Vector3 closedLocalPosition;
    private bool desiredOpenState;
    private bool revealObjectsVisible;
    private Transform clockTransform;
    private Collider2D shelfCollider;
    private Collider2D clockCollider;
    private InteractableBehaviour shelfInteractable;
    private InteractableBehaviour clockInteractable;

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
        SyncFromFlags(true);
    }

    private void Update()
    {
        ResolveTargetsIfNeeded();
        SyncFromFlags();
        MoveDoor();
        UpdateRevealObjects();
        UpdateInteractableColliders();
    }

    public void SetOpen(bool open)
    {
        desiredOpenState = open;
        UpdateInteractableColliders();
    }

    private void SyncFromFlags(bool snapToState = false)
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        var shouldBeOpen = GameManager.Instance.HasFlag(GameFlagNames.LibrarySecretDoorOpen);
        SetOpen(shouldBeOpen);
        if (snapToState)
        {
            SnapToState(shouldBeOpen);
        }
    }

    private void SnapToState(bool open)
    {
        if (doorTransform == null)
        {
            return;
        }

        doorTransform.localPosition = open
            ? closedLocalPosition + openLocalOffset
            : closedLocalPosition;
        revealObjectsVisible = open;

        if (basementDoorObject != null)
        {
            basementDoorObject.SetActive(open);
        }

        if (codePanelObject != null)
        {
            codePanelObject.SetActive(open);
        }

        UpdateInteractableColliders();
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

        var targetOpenPosition = closedLocalPosition + openLocalOffset;
        var shouldReveal = desiredOpenState
            && Vector3.Distance(doorTransform.localPosition, targetOpenPosition) <= arrivedThreshold;

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

    private void UpdateInteractableColliders()
    {
        ResolveTargetsIfNeeded();

        var targetPosition = desiredOpenState
            ? closedLocalPosition + openLocalOffset
            : closedLocalPosition;
        var isFullyOpen = desiredOpenState
            && Vector3.Distance(doorTransform.localPosition, targetPosition) <= arrivedThreshold;
        var collidersEnabled = !isFullyOpen;

        if (shelfInteractable != null)
        {
            shelfInteractable.SetColliderExternallyManaged(true);
            shelfInteractable.SetColliderEnabled(collidersEnabled);
        }
        else if (shelfCollider != null)
        {
            shelfCollider.enabled = collidersEnabled;
        }

        if (clockInteractable != null)
        {
            clockInteractable.SetColliderExternallyManaged(true);
            clockInteractable.SetColliderEnabled(collidersEnabled);
        }
        else if (clockCollider != null)
        {
            clockCollider.enabled = collidersEnabled;
        }
    }

    private void ResolveTargetsIfNeeded()
    {
        if (doorTransform != null)
        {
            if (shelfCollider == null)
            {
                shelfCollider = doorTransform.GetComponent<Collider2D>();
            }
            if (shelfInteractable == null)
            {
                shelfInteractable = doorTransform.GetComponent<InteractableBehaviour>();
            }
        }

        if (clockTransform == null)
        {
            clockTransform = FindNamedTransformInLibrary("Clock");
        }
        if (clockTransform != null)
        {
            if (clockCollider == null)
            {
                clockCollider = clockTransform.GetComponent<Collider2D>();
            }
            if (clockInteractable == null)
            {
                clockInteractable = clockTransform.GetComponent<InteractableBehaviour>();
            }
        }

        if (basementDoorObject == null)
        {
            basementDoorObject = FindNamedObjectInLibrary("BasementDoor", "Basement Door", "basement_door");
        }

        if (codePanelObject == null)
        {
            codePanelObject = FindNamedObjectInLibrary("CodePanel", "Code Panel", "code_panel");
        }
    }

    private Transform FindNamedTransformInLibrary(params string[] names)
    {
        var libraryRoot = FindLibraryRoot();
        if (libraryRoot == null)
        {
            return null;
        }

        var children = libraryRoot.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            if (child == null || !MatchesAnyName(child.gameObject.name, names))
            {
                continue;
            }

            return child;
        }

        return null;
    }

    private GameObject FindNamedObjectInLibrary(params string[] names)
    {
        var libraryRoot = FindLibraryRoot();
        if (libraryRoot == null)
        {
            return null;
        }

        var children = libraryRoot.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            if (child == null || child.gameObject == null)
            {
                continue;
            }

            if (MatchesAnyName(child.gameObject.name, names))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private GameObject FindLibraryRoot()
    {
        if (transform.parent != null)
        {
            var parent = transform.parent;
            while (parent != null)
            {
                if (parent.name.Equals("Library", System.StringComparison.OrdinalIgnoreCase))
                {
                    return parent.gameObject;
                }

                parent = parent.parent;
            }
        }

        var libraryRoot = GameObject.Find("Library");
        if (libraryRoot != null)
        {
            return libraryRoot;
        }

        var roomsParent = GameObject.Find("Rooms");
        if (roomsParent != null)
        {
            foreach (Transform child in roomsParent.transform)
            {
                if (child.name.Equals("Library", System.StringComparison.OrdinalIgnoreCase))
                {
                    return child.gameObject;
                }
            }
        }

        return null;
    }

    private static bool MatchesAnyName(string candidate, params string[] names)
    {
        foreach (var name in names)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            if (candidate.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var compactCandidate = candidate.Replace(" ", string.Empty).Replace("_", string.Empty);
            var compactName = name.Replace(" ", string.Empty).Replace("_", string.Empty);
            if (compactCandidate.Equals(compactName, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
