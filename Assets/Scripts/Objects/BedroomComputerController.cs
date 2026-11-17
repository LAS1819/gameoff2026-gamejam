using System;
using UnityEngine;

public class BedroomComputerController : MonoBehaviour
{
    [SerializeField] private SpriteStateController powerStateController;

    public bool IsPowered { get; private set; }
    public bool HasFloppyInserted { get; private set; }

    private void Awake()
    {
        if (powerStateController == null)
        {
            powerStateController = GetComponentInChildren<SpriteStateController>(true);
        }

        var poweredFlag = GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.BedroomComputerPowered);
        var floppyFlag = GameManager.Instance != null && GameManager.Instance.HasFlag(GameFlagNames.BedroomComputerDiskInserted);

        SetPowered(poweredFlag);
        SetDiskInserted(floppyFlag);
    }

    public void TogglePower()
    {
        SetPowered(!IsPowered);

        var message = IsPowered
            ? "The computer tower clicks on. The screen hums back to life."
            : "The computer tower powers down and the monitor fades to black.";

        var uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            uiManager.ShowFeedback(message, 1.4f);
        }
    }

    public bool TryInsertFloppyDisk()
    {
        if (!IsPowered)
        {
            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("Power on the tower before inserting the floppy disk.", 1.6f);
            }

            return false;
        }

        var inventory = InventoryManager.Instance;
        if (inventory == null)
        {
            return false;
        }

        if (!inventory.HasItem("floppy_disk"))
        {
            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("You do not have the floppy disk to insert.", 1.6f);
            }

            return false;
        }

        if (!string.Equals(inventory.SelectedItemId, "floppy_disk", StringComparison.OrdinalIgnoreCase))
        {
            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("Select the floppy disk first.", 1.6f);
            }

            return false;
        }

        inventory.RemoveItem("floppy_disk");
        SetDiskInserted(true);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BasementCodeAcquired, true);
        }

        var finalUiManager = UIManager.Instance;
        if (finalUiManager != null)
        {
            finalUiManager.ShowFeedback("You insert the floppy disk. The monitor flickers and reveals the basement code.", 2.2f);
        }

        return true;
    }

    public bool TryReadMonitor()
    {
        if (!IsPowered)
        {
            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("The monitor is dark. Power on the tower first.", 1.6f);
            }

            return true;
        }

        if (!HasFloppyInserted)
        {
            var uiManager = UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.ShowFeedback("The screen is waiting for a floppy disk.", 1.6f);
            }

            return true;
        }

        var codeText = "Basement access code: 4821";
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BasementCodeAcquired, true);
        }

        var readUiManager = UIManager.Instance;
        if (readUiManager != null)
        {
            readUiManager.ShowFeedback($"The monitor displays: {codeText}", 2.6f);
        }

        return true;
    }

    public void SetPowered(bool powered)
    {
        IsPowered = powered;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BedroomComputerPowered, powered);
        }

        RefreshVisualState();
    }

    public void SetDiskInserted(bool inserted)
    {
        HasFloppyInserted = inserted;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BedroomComputerDiskInserted, inserted);
        }

        if (!inserted && GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BasementCodeAcquired, false);
        }
    }

    private void RefreshVisualState()
    {
        if (powerStateController == null)
        {
            return;
        }

        powerStateController.SetState(IsPowered ? 1 : 0);
    }
}
