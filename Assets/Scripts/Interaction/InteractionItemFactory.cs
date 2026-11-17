internal static class InteractionItemFactory
{
    public static InventoryItemData CreateWornBook()
    {
        return new InventoryItemData
        {
            id = "worn_book",
            displayName = "Worn Book",
            description = "A worn book with a hidden compartment in its back pages.",
            descriptionInNormalReality = "The cover is worn, but the pages feel preserved, as if the book has been carefully kept from the world.",
            descriptionInOtherReality = "The pages are not just old: they feel archived, as if they remember a version of the house that still exists somewhere else.",
            isKeyItem = true
        };
    }

    public static InventoryItemData CreateBasementCode()
    {
        return new InventoryItemData
        {
            id = "basement_code",
            displayName = "Basement Code",
            description = "A notebook page with a basement access code written in a careful hand.",
            descriptionInNormalReality = "The code is precise and deliberate, like it was written for someone who would not forget it.",
            descriptionInOtherReality = "The same numbers feel older than the room, like a memory the house kept hidden until it was needed.",
            isKeyItem = true
        };
    }
}
