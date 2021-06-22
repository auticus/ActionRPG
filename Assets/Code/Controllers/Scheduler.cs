using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG.Interfaces;
using UnityEngine;

namespace RPG.Controllers
{
    public class Scheduler : MonoBehaviour
    {
        private IAction _currentAction;
        public void StartAction(IAction action)
        {
            if (_currentAction == action) return;

            _currentAction?.Cancel();
            _currentAction = action;
        }

        public void CancelCurrentAction()
        {
            if (_currentAction == null) return;
            _currentAction.Cancel();
            _currentAction = null;
        }
    }
}
