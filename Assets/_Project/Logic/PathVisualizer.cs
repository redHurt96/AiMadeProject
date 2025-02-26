using UnityEngine;

namespace TowerDefense
{
    public class PathVisualizerScript : MonoBehaviour
    {
        [SerializeField] private Color _pathColor = Color.green;
        [SerializeField] private float _pathWidth = 0.1f;
        [SerializeField] private Transform[] _waypoints;

        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _waypoints.Length;

            for (int i = 0; i < _waypoints.Length; i++)
            {
                _lineRenderer.SetPosition(i, _waypoints[i].position);
            }

            _lineRenderer.startWidth = _pathWidth;
            _lineRenderer.endWidth = _pathWidth;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = _pathColor;
            _lineRenderer.endColor = _pathColor;
        }

        public Transform[] GetWaypoints()
        {
            return _waypoints;
        }
    }
}