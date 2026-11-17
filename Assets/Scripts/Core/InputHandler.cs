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
        if (!IsShiftPressed())
        {
            return;
        }

        if (RealityManager.Instance == null)
        {
            return;
        }

        if (!RealityManager.Instance.IsShiftAllowedInCurrentRoom())
        {
            Debug.LogWarning($"[InputHandler] SHIFT blocked in room '{RoomManager.Instance?.CurrentRoom?.id ?? "<unknown>"}'");
            return;
        }

        RealityManager.Instance.TryToggleReality();
        Debug.Log("[InputHandler] SHIFT pressed - toggling reality");
    }

    private static bool IsShiftPressed()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }

        return keyboard.leftShiftKey.wasPressedThisFrame || keyboard.rightShiftKey.wasPressedThisFrame;
    }
}
