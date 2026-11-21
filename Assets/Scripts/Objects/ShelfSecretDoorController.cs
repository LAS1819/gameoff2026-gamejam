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

    private void ResolveTargetsIfNeeded()
    {
        if (basementDoorObject == null)
        {
            basementDoorObject = FindNamedObjectInLibrary("BasementDoor", "Basement Door", "basement_door");
        }

        if (codePanelObject == null)
        {
            codePanelObject = FindNamedObjectInLibrary("CodePanel", "Code Panel", "code_panel");
        }
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
