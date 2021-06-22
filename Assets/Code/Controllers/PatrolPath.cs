using UnityEngine;

namespace RPG.Controllers
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float wayPointRadius = 0.3f;
        private int _currentWaypointIndex = 0;

        private void OnDrawGizmos()
        {
            DrawWayPoints();
        }

        public Vector3 GetCurrentWaypoint()
        {
            return transform.childCount == 0 ? transform.position : transform.GetChild(_currentWaypointIndex).position;
        }

        public Vector3 GetNextWaypoint()
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex == transform.childCount)
                _currentWaypointIndex = 0;

            return GetCurrentWaypoint();
        }

        private void DrawWayPoints()
        {
            if (transform.childCount == 0) return;

            var firstLocation = transform.GetChild(0).position;

            for (int i = 0; i < transform.childCount; i++)
            {
                var waypoint = transform.GetChild(i);
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(waypoint.position, wayPointRadius);

                if (i > 0)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(transform.GetChild(i-1).position, waypoint.position);
                }
            }

            if (transform.childCount > 1)
            {
                Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, firstLocation);
            }
        }
    }
}