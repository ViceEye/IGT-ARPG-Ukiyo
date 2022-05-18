using UnityEngine;

namespace Task
{
    // Bind button down event to run navigation
    public class TaskButton : MonoBehaviour
    {
        public TaskData TaskData;

        public delegate void OnNavigationButtonDown(string type);
        public event OnNavigationButtonDown OnNavigationEvent;
        
        public void ButtonDown()
        {
            OnNavigationEvent?.Invoke(TaskData.Nav);
        }
    }
}