using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Player
{
    public class PathVisualization : MonoBehaviour
    {
        public GameObject target;
        [SerializeField] 
        private LineRenderer _lineRenderer;
        [SerializeField] 
        private NavMeshAgent _navMeshAgent;
        
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            
            _navMeshAgent.isStopped = true;

            _lineRenderer.startWidth = 0.15f;
            _lineRenderer.endWidth = 0.15f;
            _lineRenderer.positionCount = 0;
        }

        void Update()
        {
            if (null != target)
            {
                _navMeshAgent.SetDestination(target.transform.position);
            }
            if (_navMeshAgent.hasPath)
            {
                DrawLine();
            }
        }

        void DrawLine()
        {
            var corners = _navMeshAgent.path.corners;
            _lineRenderer.positionCount = corners.Length;
            _lineRenderer.SetPosition(0, transform.position);

            if (_navMeshAgent.path.corners.Length < 2) return;

            for (int i = 1; i < corners.Length; i++)
            {
                Vector3 pointPosition = new Vector3(corners[i].x, corners[i].y, corners[i].z);
                _lineRenderer.SetPosition(i, pointPosition);
            }
        }
        
        // FPS Visualizer Debug only
        void OnGUI()
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = 50,
                normal =
                {
                    textColor = Color.white
                }
            };
            GUI.Label(new Rect(0, 0, 100, 100), (int)(1.0f / Time.smoothDeltaTime) + "", style);
        }
    }
}
