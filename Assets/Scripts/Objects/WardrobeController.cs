using UnityEngine;

public class WardrobeController : MonoBehaviour
{
    [SerializeField] private SpriteStateController spriteStateController;
    [SerializeField] private GameObject closedInteractionObject;
    [SerializeField] private GameObject openInteractionObject;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (spriteStateController == null)
        {
            spriteStateController = GetComponentInChildren<SpriteStateController>();
        }

        RefreshVisualState();
    }

    public void SetOpen(bool open)
    {
        IsOpen = open;

        if (spriteStateController != null)
        {
            spriteStateController.SetState(open ? 1 : 0);
        }

        if (closedInteractionObject != null)
        {
            closedInteractionObject.SetActive(!open);
        }

        if (openInteractionObject != null)
        {
            openInteractionObject.SetActive(open);
        }
    }

    public void Toggle()
    {
        SetOpen(!IsOpen);
    }

    private void RefreshVisualState()
    {
        SetOpen(IsOpen);
    }
}
