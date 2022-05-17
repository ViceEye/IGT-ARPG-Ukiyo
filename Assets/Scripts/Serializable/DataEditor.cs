using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Task;
using Ukiyo.Common;
using Ukiyo.Serializable.Entity;
using UnityEditor;
using UnityEngine;

namespace Ukiyo.Serializable
{
#if UNITY_EDITOR
    public class DataEditor : MonoBehaviour
    {
        #region Item
        
        private string saveDataFilePath = "/Resources/SaveData/";
        private string itemsFileName = "EquipmentItemsData.json";
        
        [SerializeField]
        private List<ObjectData> listItemDataSettings;
        [SerializeField]
        private List<ObjectData> previewListItemDataSettings;
        
        [ContextMenu("Generate Item Json File")]
        private void GenerateItemJsonFile()
        {
            List<ObjectDataJson> listItemJsons = new List<ObjectDataJson>();
            foreach (var listItemDataSetting in listItemDataSettings)
            {
                listItemJsons.Add(new ObjectDataJson(listItemDataSetting));
            }
            Utils.WriteIntoFile(listItemJsons, saveDataFilePath, itemsFileName);
            Debug.Log("Generated Item Json");
        }

        [ContextMenu("Load Items From Json File")]
        private void LoadItemsFromJsonFile()
        {
            List<ObjectData> listItems = new List<ObjectData>();

            string allItemsJson = Utils.GetJsonStr(saveDataFilePath, itemsFileName);
            
            if (allItemsJson != "" && allItemsJson != "[{}]")
            {
                JsonData allItemsJsonData = JsonMapper.ToObject(allItemsJson);

                foreach (JsonData jsonData in allItemsJsonData)
                {
                    ObjectDataJson objectDataJson = JsonMapper.ToObject<ObjectDataJson>(jsonData.ToJson());
                    ObjectData objectData = new ObjectData(objectDataJson);
                    listItems.Add(objectData);
                }

                previewListItemDataSettings = listItems;
            }
        }

        #endregion

        #region Entity
        
        private string saveStatsFilePath = "/Resources/SaveData/Stats/";
        private string newStatsFileName = "/Entity.json";
        
        [SerializeField]
        private List<EntityStatData> entityStats = new List<EntityStatData>();

        [ContextMenu("Create A Entity Stats")]
        private void CreateEntityStatsFile()
        {
            Utils.WriteIntoFile(new EntityStat(), saveStatsFilePath, newStatsFileName);
        }

        [ContextMenu("Load Local Entity Stats")]
        private void LoadAllEntityStats()
        {
            entityStats.Clear();
            DirectoryInfo directory = new DirectoryInfo(Application.dataPath + saveStatsFilePath);
            foreach (var fileInfo in directory.GetFiles())
            {
                if (!fileInfo.Name.Contains(".meta") && fileInfo.Name.Contains(".json"))
                {
                    string str = Utils.GetJsonStr(saveStatsFilePath, fileInfo.Name);
                    EntityStat stat = JsonMapper.ToObject<EntityStat>(str);
                    entityStats.Add(new EntityStatData(fileInfo.Name, stat));
                }
            }
        }

        [ContextMenu("Save All Entity Stats")]
        private void SaveAllEntityStats()
        {
            foreach (var entityStatData in entityStats)
            {
                Utils.WriteIntoFile(entityStatData.entityStat, saveStatsFilePath, entityStatData.path);
            }
        }

        #endregion

        #region Task
        
        [SerializeField]
        private List<TaskData> listTaskDataSettings;
        [SerializeField]
        private List<TaskData> previewListTaskDataSettings;
        
        private string saveTasksFilePath = "/Resources/SaveData/Tasks/";
        private string tasksDataFileName = "TasksData.json";

        [ContextMenu("Generate Tasks Json File")]
        private void GenerateTaskJsonFile()
        {
            Debug.Log(tasksDataFileName);
            Utils.WriteIntoFile(listTaskDataSettings, saveTasksFilePath, tasksDataFileName);
            Debug.Log("Generated Task Json");
        }
        

        [ContextMenu("Load Tasks From Json File")]
        private void LoadTasksFromJsonFile()
        {
            string allItemsJson = Utils.GetJsonStr(saveTasksFilePath, tasksDataFileName);
            
            if (allItemsJson != "" && allItemsJson != "[{}]")
            {
                JsonData allItemsJsonData = JsonMapper.ToObject(allItemsJson);

                foreach (JsonData jsonData in allItemsJsonData)
                {
                    TaskData taskData = JsonMapper.ToObject<TaskData>(jsonData.ToJson());
                    previewListTaskDataSettings.Add(taskData);
                }
            }
        }
        #endregion
    }

#endif
    #region JsonDataObject
    
    [Serializable]
    public class EntityStatData
    {
        public string path;
        public EntityStat entityStat;

        public EntityStatData(string str, EntityStat stat)
        {
            path = str;
            entityStat = stat;
        }
    }

    public class ObjectDataJson
    {
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }
        
        public int Capacity { get; set; }

        public string Description { get; set; }

        public EnumInventoryItemType Type { get; set; }
        
        public ObjectDataJson() {}
        
        public ObjectDataJson(ObjectData objectData)
        {
            ID = objectData.ID;
            DisplayName = objectData.DisplayName;
            Icon = Utils.GetResourcePath(objectData.Icon);
            Capacity = objectData.Capacity;
            Description = objectData.Description;
            Type = objectData.Type;
        }

        public override string ToString()
        {
            return $"{nameof(ID)}: {ID}," +
                   $" {nameof(DisplayName)}: {DisplayName}," +
                   $" {nameof(Icon)}: {Icon}," +
                   $" {nameof(Capacity)}: {Capacity}," +
                   $" {nameof(Description)}: {Description}," +
                   $" {nameof(Type)}: {Type}";
        }
    }
    
    #endregion
}