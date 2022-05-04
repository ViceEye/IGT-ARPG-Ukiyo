using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    Debug.Log(pathFinder.gameObject.activeSelf);
                    if (!pathFinder.gameObject.activeSelf)
                    {
                        pathFinder.gameObject.transform.position = pathFinder.target.transform.position;
                        pathFinder.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}