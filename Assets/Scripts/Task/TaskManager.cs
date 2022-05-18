using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using Ukiyo.Common;
using Ukiyo.Enemy;
using Ukiyo.Player;
using Ukiyo.UI;
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
        public UIAnimation hudComponent;
        public UIAnimation panelComponent;

        // Panel variables
        public GameObject taskSlotList;
        public GameObject taskSlotObj;
        public List<Text> taskSlotObjList;
        
        private bool hudOn = true;
        [SerializeField]
        protected List<TaskData> onGoingTasks;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
            
            LoadTasksFromJsonFile();
            
            // Bind OnDeathEvent of all enemies
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in gameObjects)
            {
                EnemyController enemy = obj.GetComponent<EnemyController>();
                enemy.OnDeathEvent += ListenToEnemyDeath;
            }
            
            // Show Hud only when start
            hudComponent.PlayOpenAnimation();
            panelComponent.PlayCloseAnimation();
            
            // Load content
            SyncPanelContent();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (hudOn)
                {
                    // Show Panel, Hide Hud
                    ThirdPersonController.locking = true;
                    hudComponent.PlayCloseAnimation();
                    panelComponent.PlayOpenAnimation();
                    Cursor.visible = true;
                    hudOn = false;
                    SyncPanelContent();
                }
                else
                {
                    // Show Hud, Hide Panel
                    ThirdPersonController.locking = false;
                    hudComponent.PlayOpenAnimation();
                    panelComponent.PlayCloseAnimation();
                    Cursor.visible = false;
                    hudOn = true;
                }
            }
        }

        private void FixedUpdate()
        {
            // Update Hud Content
            SyncHudContent();
        }
        
        #region Task APIs

        private void SyncPanelContent()
        {
            // Init Phase
            if (taskSlotObjList.Count == 0)
            {
                for (var i = 0; i < onGoingTasks.Count; i++)
                {
                    TaskData task = onGoingTasks[i];
                    GameObject go = Instantiate(taskSlotObj, taskSlotList.transform);
                
                    // Bind Navigation Event
                    TaskButton taskButton = go.GetComponent<TaskButton>();
                    taskButton.TaskData = task;
                    taskButton.OnNavigationEvent += ListenToNavButton;
                
                    // Format content
                    Text text = go.GetComponentInChildren<Text>();
                    text.text = FormatTaskString(task);
                    taskSlotObjList.Add(text);

                    go.name = "Task-" + (i + 1);
                }
            }
            // Update Phase
            else
            {
                for (var i = 0; i < taskSlotObjList.Count; i++)
                {
                    // Format content
                    TaskData task = onGoingTasks[i];
                    taskSlotObjList[i].text = FormatTaskString(task);
                }
            }
        }

        private void SyncHudContent()
        {
            // Show 3 tasks by default
            // If less then 3 onGoingTasks only show the number of onGoingTasks on Hud
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
                // Format content
                TaskData taskData = onGoingTasks[i];
                tasksUI[i].text = FormatTaskString(taskData);
            }
        }
        
        private string FormatTaskString(TaskData taskData)
        {
            string baseStr = "{%id} > {%type} \n {%typeDetail} {%detail} - {%progress}/{%target}";
            baseStr = baseStr.Replace("{%id}", taskData.ID.ToString());
            baseStr = baseStr.Replace("{%type}", taskData.Type.ToString());
            baseStr = baseStr.Replace("{%typeDetail}", GetDetailByType(taskData.Type));
            baseStr = baseStr.Replace("{%detail}", taskData.Detail);
            baseStr = baseStr.Replace("{%progress}", taskData.Progress.ToString());
            baseStr = baseStr.Replace("{%target}", taskData.Target.ToString());
            return baseStr;
        }

        private string GetDetailByType(EnumTaskType type)
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
                if (includeContains && task.Detail.Contains(detail))
                    tasks.Add(task);
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
        
        #endregion

        #region Listener

        private void ListenToEnemyDeath(EnumEntityStatsType type)
        {
            // When enemy is dead, check onGoingTasks find target task and update progress
            foreach (var task in onGoingTasks)
                if (task.Type == EnumTaskType.Elimination)
                    if (task.Progress < task.Target)
                        if (task.Detail.Equals(type.ToString()))
                            task.Progress += 1;
        }

        private void ListenToNavButton(string nav)
        {
            foreach (var o in GameObject.FindGameObjectsWithTag("NavPoint"))
            {
                if (nav.Equals(o.name))
                {
                    // Run navigation
                    PathManager.Instance.GetAvailableFinder().target = o;
                    PathManager.Instance.GetAvailableFinder().player = GameObject.FindWithTag("Player");
                }
            }
        }

        #endregion

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