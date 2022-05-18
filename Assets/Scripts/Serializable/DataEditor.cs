using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using Task;
using Ukiyo.Common;
using Ukiyo.Serializable.Entity;
using UnityEngine;

namespace Ukiyo.Serializable
{
    public class DataEditor : MonoBehaviour
    {
        
        private string saveDataFilePath = "/Resources/SaveData/";
        
        private void Start()
        {
            // Load preset configurations into local when there is none
            string path = Application.dataPath + saveDataFilePath;
            if (!Directory.Exists(path))
            {
                InitLocalItemJsons();
                SaveAllEntityStats();
                GenerateTaskJsonFile();
                Debug.LogError("Generated");
            }
            else
            {
                Debug.LogError("No need generate");
            }
        }
        
        #region Item
        
        private string itemsFileName = "EquipmentItemsData.json";
        
        [SerializeField]
        private List<ObjectData> listItemDataSettings = new List<ObjectData>();
        [SerializeField]
        private List<ObjectDataJson> previewListItemDataSettings = new List<ObjectDataJson>();

// ObjectDataJson contains library only runs in editor
#if UNITY_EDITOR
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
#endif

        [ContextMenu("Load Items From Json File")]
        private void LoadItemsFromJsonFile()
        {
            List<ObjectDataJson> listItems = new List<ObjectDataJson>();

            string allItemsJson = Utils.GetJsonStr(saveDataFilePath, itemsFileName);
            Debug.Log(allItemsJson);
            
            if (allItemsJson != "" && allItemsJson != "[{}]")
            {
                JsonData allItemsJsonData = JsonMapper.ToObject(allItemsJson);

                foreach (JsonData jsonData in allItemsJsonData)
                {
                    Debug.Log(jsonData.ToJson());
                    ObjectDataJson objectDataJson = JsonMapper.ToObject<ObjectDataJson>(jsonData.ToJson());
                    //ObjectData objectData = new ObjectData(objectDataJson);
                    listItems.Add(objectDataJson);
                }

                previewListItemDataSettings = listItems;
            }
        }

        private void InitLocalItemJsons()
        {
            Utils.WriteIntoFile(previewListItemDataSettings, saveDataFilePath, itemsFileName);
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

    [Serializable]
    public class ObjectDataJson
    {
        [SerializeField]
        protected int _id;
        public int ID { get => _id; set => _id = value; }
        
        [SerializeField]
        protected string _displayName;
        public string DisplayName { get => _displayName; set => _displayName = value; }
        
        [SerializeField]
        protected string _icon;
        public string Icon { get => _icon; set => _icon = value; }
        
        [SerializeField]
        protected int _capacity;
        public int Capacity { get => _capacity; set => _capacity = value; }

        [SerializeField]
        protected string _description;
        public string Description { get => _description; set => _description = value; }

        [SerializeField]
        protected EnumInventoryItemType _type;
        public EnumInventoryItemType Type { get => _type; set => _type = value; }
        
        public ObjectDataJson() {}
        
// ObjectDataJson contains library only runs in editor
#if UNITY_EDITOR
        public ObjectDataJson(ObjectData objectData)
        {
            ID = objectData.ID;
            DisplayName = objectData.DisplayName;
            Icon = Utils.GetResourcePath(objectData.Icon);
            Capacity = objectData.Capacity;
            Description = objectData.Description;
            Type = objectData.Type;
        }
#endif

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