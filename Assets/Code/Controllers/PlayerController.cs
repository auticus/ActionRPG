using System;
using RPG.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace RPG.Controllers
{
    public class PlayerController : EntityController
    {
        [Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings = null;
        [SerializeField] private float navMeshRange = 1.0f; //the nearest point based on the navmesh within this range
        [SerializeField] private float maxNavPathLength = 40f;  //the max path we want to let the player click to in one go

        void Start()
        {
            Initialize();
        }

        private void Update()
        {
            //first - interact with UI
            if (InteractWithUI()) return; //in this case do not do anything
            if (HandleDeath())
            {
                SetCursor(CursorType.None); //if i'm dead... show them the no no cursor
                return;
            }

            if (InteractableWithComponent()) return;
            if (Movement()) return;

            SetCursor(CursorType.None);
        }

        private bool Movement()
        {
            var hasHitNavMesh = IsRaycastNavMesh(out var target);
            if (hasHitNavMesh)
            {
                if (Input.GetMouseButton(0))
                {
                    movement.StartMoveAction(target);
                }

                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private bool IsRaycastNavMesh(out Vector3 target)
        {
            //sample position on the navmesh to determine if we're on a valid part of it or near enough to it
            target = Vector3.zero;
            //raycast to terrain
            var ray = FireRayFromCamera();
            var hasHit = Physics.Raycast(ray, out var hit);
            if (!hasHit) return false;

            //find nearest navmesh point within a specified range
            if (!NavMesh.SamplePosition(sourcePosition: hit.point, out var navMeshHit, navMeshRange, NavMesh.AllAreas))
            {
                return false;
            }

            target = navMeshHit.position;

            //just because we have a point doesn't mean we want to use it.  we do not want to have the player running across the map on a 
            //massive jaunt because the navmesh could figure it out but its far away
            //example: flat buildings may show nav mesh on the building.  This will prevent that from being legit by returning false since its not complete
            var navMeshPath = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, navMeshPath);

            if (!hasPath) return false;
            if (navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(navMeshPath) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            var total = 0.0f;
            if (path.corners.Length < 2) return total; //we dont have anything to sum here

            for (int i = 0; i < path.corners.Length - 1; i++) //length - 1 because that way when we are putting the fence posts together, we dont get to the last one that has no next one
            {
                var distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                total += distance;
            }

            return total;
        }

        private Ray FireRayFromCamera()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithUI()
        {
            //fun times - the Fader is a persistant object and a canvas which is ... a UI object which will always be present blocking you now
            //make sure that the canvas is not blocking raycasts and is not interactable (it is by default)

            var isOverUI = EventSystem.current.IsPointerOverGameObject(); //this is confusing but it means is over UI element
            if (isOverUI)
            {
                SetCursor(CursorType.UI);
            }

            return isOverUI;
        }

        private void SetCursor(CursorType cursor)
        {
            var mapping = GetCursorMapping(cursor);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursor)
        {
            for (var i = 0; i < cursorMappings.Length; i++)
            {
                if (cursorMappings[i].cursorType == cursor)
                {
                    return cursorMappings[i];
                }
            }

            Debug.LogWarning($"The type {cursor} does not exist in the CursorMapping on the player controller");
            return new CursorMapping();
        }

        private bool InteractableWithComponent()
        {
            //this was modified so that instead of a specific combat and then item piece, we consolidate them all into the IRaycastable interaface
            var hits = SortedRaycast();
            foreach (var hit in hits)
            {
                var castables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in castables)
                {
                    if (!raycastable.HandleRaycast(this)) continue;
                    var cursorType = raycastable.GetCursorType();
                    SetCursor(cursorType);
                    return true;
                }
            }

            return false;
        }

        private RaycastHit[] SortedRaycast()
        {
            //if we have multiple racyastable hit objects in the same z plane this can cause some problems
            //so we sort them and return them in the order that they are distance wise so that its sorted out
            var hits = Physics.RaycastAll(FireRayFromCamera());

            //build array of distances to sort
            var distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            //sort the hits
            Array.Sort(distances, hits);
            return hits;
        }
    }
}
