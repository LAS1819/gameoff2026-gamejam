using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public string id;
    public string displayName;
    public string description;
    public string descriptionInNormalReality;
    public string descriptionInOtherReality;
    public bool isKeyItem;

    public InventoryItemData()
    {
    }

    public InventoryItemData(string id, string displayName, string description, bool isKeyItem = false)
    {
        this.id = id;
        this.displayName = displayName;
        this.description = description;
        this.isKeyItem = isKeyItem;
    }

    public string GetDescriptionForReality(RealityType reality)
    {
        if (reality == RealityType.Normal && !string.IsNullOrWhiteSpace(descriptionInNormalReality))
        {
            return descriptionInNormalReality;
        }

        if (reality == RealityType.OtherSide)
        {
            if (!string.IsNullOrWhiteSpace(descriptionInOtherReality))
            {
                return descriptionInOtherReality;
            }

            if (!string.IsNullOrWhiteSpace(descriptionInNormalReality))
            {
                return descriptionInNormalReality;
            }
        }

        return string.IsNullOrWhiteSpace(description) ? displayName : description;
    }
}

public enum InteractionType
{
    Examine,
    Pickup,
    Use,
    Transition,
    Toggle
}

public enum RealityType
{
    Normal,
    OtherSide
}

[Serializable]
public class InteractableData
{
    public string id;                               // Unique identifier for this interactable
    public string displayName;
    public string description;
    public string descriptionInNormalReality;
    public string descriptionInOtherReality;
    public string memoryId;
    public InteractionType interactionType;
    
    // Visibility rules for reality system
    public bool visibleInNormalReality = true;
    public bool visibleInOtherReality = true;
    
    // For Pickup interactions: what item does the player receive?
    public string itemId;
    
    // For Use interactions: what item is required to use this?
    public string requiredItemId;

    // Should the required item be consumed when interaction succeeds?
    public bool consumeRequiredItemOnUse;
    
    // What flag to set when this interaction succeeds?
    public string resultFlag;
    
    // For Transition interactions: which room to go to?
    public string linkedRoomId;
    
    // Should this interactable be consumed after first use? (e.g., key disappears after unlocking door)
    public bool consumeAfterUse;

    // Require the flashlight to be obtained and active before resolving.
    public bool requiresLantern;

    public string GetDescriptionForReality(RealityType reality)
    {
        if (reality == RealityType.Normal && !string.IsNullOrWhiteSpace(descriptionInNormalReality))
        {
            return descriptionInNormalReality;
        }

        if (reality == RealityType.OtherSide)
        {
            if (!string.IsNullOrWhiteSpace(descriptionInOtherReality))
            {
                return descriptionInOtherReality;
            }

            if (!string.IsNullOrWhiteSpace(descriptionInNormalReality))
            {
                return descriptionInNormalReality;
            }
        }

        return string.IsNullOrWhiteSpace(description) ? displayName : description;
    }
}

[Serializable]
public class RoomData
{
    public string id;
    public string displayName;
    public List<InteractableData> interactables = new List<InteractableData>();
    public List<string> connectedRoomIds = new List<string>();
    public RoomLightingProfile lightingProfile;
}

[Serializable]
public class RoomLightingProfile
{
    public float normalIntensity = 1f;
    public Color normalColor = Color.white;
    public float otherSideIntensity = 1f;
    public Color otherSideColor = Color.white;
    public bool lanternAllowed;
    public float lanternNormalIntensity = 0.28f;
    public float lanternOtherSideIntensity = 0.22f;
}

[Serializable]
public class MemoryDefinition
{
    public string id;
    public string title;
    public string body;
    public float duration;

    public MemoryDefinition(string id, string title, string body, float duration = 10f)
    {
        this.id = id;
        this.title = title;
        this.body = body;
        this.duration = duration;
    }
}

public static class MemoryCatalog
{
    private static readonly Dictionary<string, MemoryDefinition> Definitions =
        new Dictionary<string, MemoryDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            {
                GameFlagNames.Memory_01,
                new MemoryDefinition(
                    GameFlagNames.Memory_01,
                    "First Steps",
                    "I remember walking beside him, when the whole world seemed larger than both of us.")
            },
            {
                GameFlagNames.Memory_02,
                new MemoryDefinition(
                    GameFlagNames.Memory_02,
                    "The Fossil",
                    "Even the things that disappear leave marks. Father said the past can remain for a very long time.")
            },
            {
                GameFlagNames.Memory_03,
                new MemoryDefinition(
                    GameFlagNames.Memory_03,
                    "The Closed Window",
                    "I shut myself away. He stayed outside the door, patient, until I was ready to open it.")
            },
            {
                GameFlagNames.Memory_04,
                new MemoryDefinition(
                    GameFlagNames.Memory_04,
                    "11:30",
                    "At 11:30, we talked about ordinary things. It was the last conversation I never knew would end.")
            },
            {
                GameFlagNames.Memory_05,
                new MemoryDefinition(
                    GameFlagNames.Memory_05,
                    "The Orchard",
                    "His hands worked the soil with quiet pride. For a while, helping others made him truly happy.")
            }
        };

    public static bool TryGet(string memoryId, out MemoryDefinition definition)
    {
        if (string.IsNullOrWhiteSpace(memoryId))
        {
            definition = null;
            return false;
        }

        return Definitions.TryGetValue(memoryId, out definition);
    }
}
