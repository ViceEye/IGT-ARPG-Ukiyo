using System;
using System.Collections.Generic;
using LitJson;
using Ukiyo.Common.Singleton;
using Ukiyo.Serializable;
using Ukiyo.Serializable.Entity;

namespace Ukiyo.Common.Object
{
    public class ObjectPool : MonoSingleton<ObjectPool>, IObject
    {
        private string saveDataFilePath = "/Resources/SaveData/";
        private string itemsFileName = "EquipmentItemsData.json";
        
        private string saveStatsFilePath = "/Resources/SaveData/Stats/";
        
        private readonly Dictionary<int, ObjectData> itemPool = new Dictionary<int, ObjectData>();
        private readonly Dictionary<EnumEntityStatsType, EntityStat> statsPool = new Dictionary<EnumEntityStatsType, EntityStat>();

        private Random _random = new Random();
        public Random Random => _random;
        
        public void Init()
        {
            LoadItemsFromJsonFile();
            LoadAllStatsFromJsonFile();
        }

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }

        #region Item
        
        // Load Al Items From Json File and cache to Item Pool
        private void LoadItemsFromJsonFile()
        {
            string allItemsJson = Utils.GetJsonStr(saveDataFilePath, itemsFileName);

            if (allItemsJson != "" && allItemsJson != "[{}]")
            {
                JsonData allItemsJsonData = JsonMapper.ToObject(allItemsJson);

                foreach (JsonData jsonData in allItemsJsonData)
                {
                    ObjectDataJson objectDataJson = JsonMapper.ToObject<ObjectDataJson>(jsonData.ToJson());
                    ObjectData objectData = new ObjectData(objectDataJson);
                    // Cache item to item pool
                    if (!itemPool.ContainsKey(objectData.ID))
                        itemPool.Add(objectData.ID, objectData);
                }
            }
        }

        public ObjectData GetItemById(int id)
        {
            if (itemPool.ContainsKey(id))
            {
                return itemPool[id];
            }
            return null;
        }

        #endregion


        #region Stats

        private void LoadAllStatsFromJsonFile()
        {
            string characterKenString = Utils.GetJsonStr(saveStatsFilePath, StatsDefine.CHARACTER_KEN_STATS);
            statsPool.Add(EnumEntityStatsType.CharacterKen, JsonMapper.ToObject<EntityStat>(characterKenString));
            
            string golemString = Utils.GetJsonStr(saveStatsFilePath, StatsDefine.GOLEM_STATS);
            statsPool.Add(EnumEntityStatsType.Golem, JsonMapper.ToObject<EntityStat>(golemString));
            
            string monsterPlantString = Utils.GetJsonStr(saveStatsFilePath, StatsDefine.MONSTER_PLANT_STATS);
            statsPool.Add(EnumEntityStatsType.MonsterPlant, JsonMapper.ToObject<EntityStat>(monsterPlantString));

            string orcString = Utils.GetJsonStr(saveStatsFilePath, StatsDefine.ORC_STATS);
            statsPool.Add(EnumEntityStatsType.Orc, JsonMapper.ToObject<EntityStat>(orcString));

            string skeletonString = Utils.GetJsonStr(saveStatsFilePath, StatsDefine.SKELETON_STATS);
            statsPool.Add(EnumEntityStatsType.Skeleton, JsonMapper.ToObject<EntityStat>(skeletonString));
            
        }

        public EntityStat GetStatByType(EnumEntityStatsType type)
        {
            if (statsPool.ContainsKey(type))
            {
                return statsPool[type];
            }
            return null;
        }

        #endregion
    }
}