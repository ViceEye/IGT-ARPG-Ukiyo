using System.Collections.Generic;
using UnityEngine;

namespace Ukiyo.Player
{
    public class PathManager : MonoBehaviour
    {

        public static PathManager Instance;
        
        public List<PathVisualization> _pathFinders = new List<PathVisualization>();
        public float _distance = 3;
        
        private void Start()
        {
            if (Instance == null)
                Instance = this;
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

                    if (pathFinder.gameObject.activeSelf)
                    {
                        if (null != pathFinder.target && null != pathFinder.player)
                            // Stop when close to target
                            if (Vector3.Distance(pathFinder.player.transform.position,
                                    pathFinder.target.transform.position) < _distance)
                            {
                                pathFinder.target = null;
                                pathFinder.gameObject.SetActive(false);
                            }
                    }
                }
            }
        }

        public PathVisualization GetAvailableFinder()
        {
            // Get first one, since we only need one finder for now, so always first one
            foreach (var pathFinder in _pathFinders)
                return pathFinder;
            return null;
        }
    }
}