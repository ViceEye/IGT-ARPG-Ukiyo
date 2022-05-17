using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using Ukiyo.Common;
using Ukiyo.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace Task
{
    public class TaskManager : MonoBehaviour
    {
        private string saveTasksFilePath = "/Resources/SaveData/Tasks/";
        private string tasksDataFileName = "TasksData.json";
        
        private Text[] copyTasksUI = new Text[3];
        private List<Text> reverseTasksUI = new List<Text>();
        
        
        public static TaskManager Instance;

        public List<Text> tasksUI;
        [SerializeField]
        protected List<TaskData> onGoingTasks;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
            LoadTasksFromJsonFile();
            EnemyController.OnDeathEvent += ListenToEnemyDeath;
            ListenToEnemyDeath(EnumEntityStatsType.Orc);
        }
        
        private void FixedUpdate()
        {
            if (onGoingTasks.Count < 3)
            {
                int numbersOfTasksUIToDisable = 3 - onGoingTasks.Count;
                tasksUI.CopyTo(copyTasksUI); 
                reverseTasksUI = copyTasksUI.ToList();
                reverseTasksUI.Reverse();
                for (var i = 0; i < numbersOfTasksUIToDisable ; i++)
                    reverseTasksUI[i].gameObject.SetActive(false);
            }

            for (var i = 0; i < onGoingTasks.Count; i++)
            {
                TaskData taskData = onGoingTasks[i];
                tasksUI[i].text = formatTaskString(taskData);
            }
        }

        private string formatTaskString(TaskData taskData)
        {
            // 1 > Elimination
            // Kill Orc - 5/5
            string baseStr = "{%id} > {%type} \n {%typeDetail} {%detail} - {%progress}/{%target}";
            baseStr = baseStr.Replace("{%id}", taskData.ID.ToString());
            baseStr = baseStr.Replace("{%type}", taskData.Type.ToString());
            baseStr = baseStr.Replace("{%typeDetail}", getDetailByType(taskData.Type));
            baseStr = baseStr.Replace("{%detail}", taskData.Detail);
            baseStr = baseStr.Replace("{%progress}", taskData.Progress.ToString());
            baseStr = baseStr.Replace("{%target}", taskData.Target.ToString());
            return baseStr;
        }

        private string getDetailByType(EnumTaskType type)
        {
            switch (type)
            {
                case EnumTaskType.Chat:
                    return "Chat with";
                case EnumTaskType.Move:
                    return "Move to";
                case EnumTaskType.Collection:
                    return "Collect";
                case EnumTaskType.Elimination:
                    return "Kill";
                default:
                    return "";
            }
        }

        public TaskData GetTaskByID(int id)
        {
            foreach (var task in onGoingTasks)
                if (task.ID == id)
                    return task;
            return null;
        }

        public List<TaskData> GetTaskByDetail(string detail, bool includeContains = false)
        {
            List<TaskData> tasks = new List<TaskData>();
            foreach (var task in onGoingTasks)
            {
                if (task.Detail.Equals(detail))
                {
                    tasks.Add(task);
                    continue;
                }
                if (includeContains)
                {
                    if (task.Detail.Contains(detail))
                    {
                        tasks.Add(task);
                    }
                }
            }
            return tasks;
        }

        public List<TaskData> GetTasksByType(EnumTaskType type)
        {
            List<TaskData> tasks = new List<TaskData>();
            foreach (var task in onGoingTasks)
                if (task.Type == type)
                    tasks.Add(task);
            return tasks;
        }

        private void ListenToEnemyDeath(EnumEntityStatsType type)
        {
            foreach (var task in onGoingTasks)
                if (task.Type == EnumTaskType.Elimination)
                    if (task.Progress < task.Target)
                        if (task.Detail.Equals(nameof(type)))
                            task.Progress += 1;
        }

        private void LoadTasksFromJsonFile()
        {
            string allItemsJson = Utils.GetJsonStr(saveTasksFilePath, tasksDataFileName);
            if (allItemsJson != "" && allItemsJson != "[{}]")
            {
                JsonData allItemsJsonData = JsonMapper.ToObject(allItemsJson);

                foreach (JsonData jsonData in allItemsJsonData)
                {
                    TaskData taskData = JsonMapper.ToObject<TaskData>(jsonData.ToJson());
                    onGoingTasks.Add(taskData);
                }
            }
        }
    }
}