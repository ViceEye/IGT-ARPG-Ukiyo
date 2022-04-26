namespace Ukiyo.Common
{
    
    public enum EnumInventoryItemType
    {
        Content,    // All items
        Equipment,  // All equipment
        Consumable  // All Consumable
    }

    public static class UIDefines
    {
        #region Prefab Path
        
        public static readonly string UI_AVATAR_STATUS = "UI Prefabs/Inventory";
        
        public static readonly string UI_Inventory_PANEL = "UI Prefabs/Inventory";

        public static readonly string UI_Inventory_Slot = "UI Prefabs/Slot";

        public static readonly string UI_Inventory_Item = "UI Prefabs/Item";
        
        public static readonly string UI_PICKED_ITEM = "UI Prefabs/UIPickedItem";
        
        public static readonly string UI_TOOL_TIP = "UI Prefabs/UIToolTip";

        #endregion
    }
}