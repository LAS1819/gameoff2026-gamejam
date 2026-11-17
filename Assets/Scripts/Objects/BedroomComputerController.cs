using System;
using UnityEngine;

public class BedroomComputerController : MonoBehaviour
{
    private const string FloppyDiskItemId = "floppy_disk";
    private const string BasementCodeItemId = "basement_code";
    private const string BasementCodeAliasItemId = "code_basement";

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

        ShowFeedback(message, 1.4f);
    }

    public bool TryInsertFloppyDisk()
    {
        var inventory = InventoryManager.Instance;
        if (inventory == null)
        {
            return false;
        }

        if (!IsPowered)
        {
            ShowFeedback("Power on the tower before inserting the floppy disk.", 1.6f);
            if (!string.IsNullOrEmpty(inventory.SelectedItemId))
            {
                inventory.DeselectSelectedItem();
            }

            return false;
        }

        if (!inventory.HasItem(FloppyDiskItemId))
        {
            ShowFeedback("You do not have the floppy disk to insert.", 1.6f);
            return false;
        }

        if (!string.Equals(inventory.SelectedItemId, FloppyDiskItemId, StringComparison.OrdinalIgnoreCase))
        {
            ShowFeedback("Select the floppy disk first.", 1.6f);
            return false;
        }

        inventory.RemoveItem(FloppyDiskItemId);
        SetDiskInserted(true);
        AwardBasementCode();

        ShowFeedback("You insert the floppy disk. The monitor flickers and reveals the basement code.", 2.2f);
        return true;
    }

    public bool TryReadMonitor()
    {
        if (!IsPowered)
        {
            ShowFeedback("The monitor is dark. Power on the tower first.", 1.6f);
            return true;
        }

        if (!HasFloppyInserted)
        {
            ShowFeedback("The screen is waiting for a floppy disk.", 1.6f);
            return true;
        }

        var codeText = "Basement access code: 4821";
        AwardBasementCode();

        ShowFeedback($"The monitor displays: {codeText}", 2.6f);
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

    private void AwardBasementCode()
    {
        var inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
        {
            return;
        }

        var hasCode = inventoryManager.HasItem(BasementCodeItemId)
            || inventoryManager.HasItem(BasementCodeAliasItemId);
        if (!hasCode)
        {
            inventoryManager.AddItem(InteractionItemFactory.CreateBasementCode());
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFlag(GameFlagNames.BasementCodeAcquired, true);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshInventoryUI();
        }
    }

    private static void ShowFeedback(string message, float duration)
    {
        var uiManager = UIManager.Instance;
        if (uiManager != null)
        {
            uiManager.ShowFeedback(message, duration);
        }
    }
}
