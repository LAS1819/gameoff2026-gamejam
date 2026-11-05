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
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            RealityManager.Instance?.ToggleReality();
            Debug.Log("[InputHandler] LEFT SHIFT pressed - toggling reality");
        }
    }
}
