using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Ukiyo.Common.Singleton;
using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Player
{
    public class PathVisualization : MonoBehaviour
    {
        public GameObject player;
        public GameObject target;
        public NavMeshPathStatus status;
        [SerializeField] 
        private LineRenderer _lineRenderer;
        [SerializeField] 
        private NavMeshAgent _navMeshAgent;
        private NavMeshPath _navMeshPath;
        [Description("The lower number the more precise of path when be generated, but more calculation needs")]
        public float offset = 2.0f;
        
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            
            _lineRenderer = GetComponent<LineRenderer>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshPath = new NavMeshPath();
            
            _navMeshAgent.isStopped = true;

            _lineRenderer.startWidth = 0.15f;
            _lineRenderer.endWidth = 0.15f;
            _lineRenderer.positionCount = 0;
            
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (null != target)
            {
                // Calculate ground point of player
                RaycastHit hit = new RaycastHit();
                // Run raycast from up to bottom to determine the ground point
                // Avoid invalid path because the player is in the air
                Physics.Raycast(player.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 10.0f);
                
                _navMeshAgent.CalculatePath(hit.point, _navMeshPath);
                status = _navMeshPath.status;
                
                // Only draw line when path is not invalid.
                if (status != NavMeshPathStatus.PathInvalid)
                    DrawLine();
            }
        }

        // Debug values
        public List<Vector3> points = new List<Vector3>();
        public List<Vector3> hits = new List<Vector3>();

        void DrawLine()
        {
            // In case of memory leak
            if (offset == 0)
                offset = 1.0f;
            
            // Sync with target position
            _navMeshAgent.gameObject.transform.position = target.transform.position;
            
            // Corners calculated by nav mesh agent
            var corners = _navMeshPath.corners;
            // Points calculated for ground attachment
            // List<Vector3> points = new List<Vector3>();
            points.Clear();

            // Init variables
            Vector3? startPoint = null;
            Vector3? endPoint = null;
            
            for (var i = 0; i < corners.Length; i++)
            {
                // When the for loop starts, set the first element as the starting point
                if (i == 0 && startPoint == null)
                {
                    startPoint = corners[i];
                }
                // Set the remaining element as the end point of the previous point
                if (i != 0 && endPoint == null)
                {
                    endPoint = corners[i];
                }
                
                // When both point is not null, start calculation
                if (startPoint != null && endPoint != null)
                {
                    // Set the point higher so that it casts rays from top to bottom
                    startPoint += Vector3.up * 2;
                    endPoint += Vector3.up * 2;
                    
                    Vector3 v3 = (Vector3) startPoint;
                    // Direction of start point to end point
                    Vector3 dir = ((Vector3) (endPoint - startPoint)).normalized;
                    // First point
                    points.Add(v3);
                    
                    // Stop loop when the point is reached end point
                    while (Vector3.Distance(v3, (Vector3) endPoint) >= offset * 2)
                    {
                        // By adding a offset point to the start point towards end point, and saving it to the list
                        v3 += dir * offset;
                        points.Add(v3);
                    }
                }

                // If this is last element, straight to the list
                if (i == corners.Length - 1)
                {
                    points.Add(corners[i]);
                }
                
                // Set the start point as end point and set the end point to null
                startPoint = corners[i];
                endPoint = null;
            }

            // Raycast list
            //List<Vector3> hits = new List<Vector3>();
            hits.Clear();
            foreach (var vector3 in points)
            {
                RaycastHit hit = new RaycastHit();
                // Run raycast from up to bottom and save it to the list
                Physics.Raycast(vector3, Vector3.down, out hit, 5.0f);
                hits.Add(hit.point);
            }

            // Render the line by the Raycast list
            // + Vector3.up * 0.1f increase from the ground a little to have better visuals
            _lineRenderer.positionCount = hits.Count;
            _lineRenderer.SetPosition(0, transform.position + Vector3.up * 0.1f);

            if (_navMeshPath.corners.Length < 2) return;

            for (int i = 1; i < hits.Count; i++)
            {
                if (i == hits.Count - 1)
                {
                    _lineRenderer.SetPosition(i, player.transform.position + Vector3.up * 0.1f);
                    break;
                }
                _lineRenderer.SetPosition(i, hits[i] + Vector3.up * 0.1f);
            }
        }
        
#if UNITY_EDITOR
        // FPS Visualizer Debug only
        void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 100, 100), (int)(1.0f / Time.smoothDeltaTime) + "");
        }
#endif
    }
}
