﻿using RPG.Combat;
using UnityEngine;

namespace RPG.Controllers
{
    public class PlayerController : EntityController
    {
        void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (HandleDeath()) return;
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
                    movement.StartMoveAction(hit.point);
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
                if (target == null)
                {
                    continue;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!fighter.IsValidTarget(target))
                    {
                        continue;
                    }
                    
                    fighter.Attack(target);
                }
                return true;
            }

            return false;
        }
    }
}