using System.Collections.Generic;
using Ukiyo.Common.Singleton;
using UnityEngine;

namespace Ukiyo.Player
{
    public class PathManager : MonoSingleton<PathManager>
    {
        public List<PathVisualization> _pathFinders = new List<PathVisualization>();
        
        private void Start()
        {
            foreach (var o in GameObject.FindGameObjectsWithTag("PathFinder"))
            {
                _pathFinders.Add(o.GetComponent<PathVisualization>());
            }
        }

        private void Update()
        {
            foreach (var pathFinder in _pathFinders)
            {
                if (pathFinder.target != null)
                {   
                    if (!pathFinder.gameObject.activeSelf)
                    {
                        // Set the position of the path finder before activation
                        // Prevent agents from getting stuck on obstacles
                        pathFinder.gameObject.transform.position = pathFinder.target.transform.position;
                        pathFinder.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}