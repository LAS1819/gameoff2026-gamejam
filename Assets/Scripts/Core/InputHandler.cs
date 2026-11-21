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

        if (UIManager.Instance != null && UIManager.Instance.IsStoryOverlayVisible())
        {
            return;
        }

        if (RealityManager.Instance == null)
        {
            return;
        }

        if (!RealityManager.Instance.IsShiftAllowedInCurrentRoom())
        {
            var roomId = "<unknown>";
            if (RoomManager.Instance != null && RoomManager.Instance.CurrentRoom != null)
            {
                roomId = RoomManager.Instance.CurrentRoom.id;
            }

            Debug.LogWarning($"[InputHandler] SHIFT blocked in room '{roomId}'");
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
