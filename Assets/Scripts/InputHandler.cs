using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private void Update()
    {
        HandleShiftInput();
    }

    private void HandleShiftInput()
    {
        if (Keyboard.current == null || !Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            return;
        }

        if (RealityManager.Instance == null)
        {
            return;
        }

        if (!RealityManager.Instance.IsShiftAllowedInCurrentRoom())
        {
            Debug.LogWarning($"[InputHandler] LEFT SHIFT blocked in room '{RoomManager.Instance?.CurrentRoom?.id ?? "<unknown>"}'");
            return;
        }

        RealityManager.Instance.TryToggleReality();
        Debug.Log("[InputHandler] LEFT SHIFT pressed - toggling reality");
    }
}
