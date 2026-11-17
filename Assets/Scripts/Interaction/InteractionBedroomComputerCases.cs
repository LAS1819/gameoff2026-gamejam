using System;
using UnityEngine;

internal static class InteractionBedroomComputerCases
{
    public static bool ResolveTower(InteractableData interactable, GameObject sourceObject)
    {
        var controller = ResolveComputerController(sourceObject);
        if (controller == null)
        {
            return false;
        }

        var inventory = InventoryManager.Instance;
        var selectedItemId = inventory != null ? inventory.SelectedItemId : null;

        if (!string.IsNullOrWhiteSpace(selectedItemId)
            && selectedItemId.Equals("floppy_disk", StringComparison.OrdinalIgnoreCase))
        {
            return controller.TryInsertFloppyDisk();
        }

        controller.TogglePower();
        return true;
    }

    public static bool ResolveMonitor(InteractableData interactable, GameObject sourceObject)
    {
        var controller = ResolveComputerController(sourceObject);
        if (controller == null)
        {
            return false;
        }

        return controller.TryReadMonitor();
    }

    private static BedroomComputerController ResolveComputerController(GameObject sourceObject)
    {
        if (sourceObject != null)
        {
            var direct = sourceObject.GetComponent<BedroomComputerController>();
            if (direct != null)
            {
                return direct;
            }

            direct = sourceObject.GetComponentInParent<BedroomComputerController>();
            if (direct != null)
            {
                return direct;
            }
        }

        var computerControllers = UnityEngine.Object.FindObjectsByType<BedroomComputerController>(FindObjectsSortMode.None);
        foreach (var controller in computerControllers)
        {
            if (controller != null)
            {
                return controller;
            }
        }

        return null;
    }
}
