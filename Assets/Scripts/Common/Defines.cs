namespace Ukiyo.Common
{
    
    public enum EnumInventoryItemType
    {
        Content,    // All items
        Equipment,  // All equipment
        Consumable  // All Consumable
    }

    public static class PrefabDefines
    {
        #region UI Prefab Path
        
        public static readonly string UI_AVATAR_STATUS = "UI Prefabs/Inventory";
        
        public static readonly string UI_INVENTORY_PANEL = "UI Prefabs/Inventory";

        public static readonly string UI_INVENTORY_SLOT = "UI Prefabs/Slot";

        public static readonly string UI_INVENTORY_ITEM = "UI Prefabs/Item";
        
        public static readonly string UI_PICKED_ITEM = "UI Prefabs/UIPickedItem";
        
        public static readonly string UI_TOOL_TIP = "UI Prefabs/UIToolTip";

        #endregion

        #region Actor Prefab Path

        public static readonly string PATHFINDER = "Prefab/PathFinder";
        
        public static readonly string SWORD_CHARACTER = "Prefab/Players/Sword_Ken";

        #endregion
    }

    public enum EnumEntityStatsType
    {
        CharacterKen,
        Golem,
        MonsterPlant,
        Orc,
        Skeleton,
    }

    public static class StatsDefine
    {
        public static readonly string STATS_PATH = "/Resources/SaveData/Stats/";
        
        public static readonly string CHARACTER_KEN_STATS = "CharacterKenStats.json";
        
        public static readonly string GOLEM_STATS = "GolemStats.json";
        
        public static readonly string MONSTER_PLANT_STATS = "MonsterPlantStats.json";
        
        public static readonly string ORC_STATS = "OrcStats.json";
        
        public static readonly string SKELETON_STATS = "SkeletonStats.json";
    }
    
    public enum EnumTaskType
    {
        Chat,
        Move,
        Collection,
        Elimination,
    }
}