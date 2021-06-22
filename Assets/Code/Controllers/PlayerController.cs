using RPG.Character;
using RPG.Combat;
using UnityEngine;

namespace RPG.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private Movement _movement;
        private Fighter _fighter;

        private void Start()
        {
            _movement = GetComponent<Movement>();
            _fighter = GetComponent<Fighter>();
        }
        private void Update()
        {
            if (Combat()) return;
            if (Movement()) return;
        }

        private bool Movement()
        {
            var ray = FireRayFromCamera();
            var hasHit = Physics.Raycast(ray, out var hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _movement.StartMoveAction(hit.point);
                }

                return true;
            }

            return false;
        }

        private Ray FireRayFromCamera()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool Combat()
        {
            var hits = Physics.RaycastAll(FireRayFromCamera());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<Target>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    if (!_fighter.IsValidTarget(target))
                    {
                        continue;
                    }
                    
                    _fighter.Attack(target);
                }
                return true;
            }

            return false;
        }
    }
}
