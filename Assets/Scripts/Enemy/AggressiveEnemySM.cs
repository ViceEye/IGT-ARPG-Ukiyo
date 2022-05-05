using Ukiyo.Common.FSM;
using Ukiyo.Enemy.FSM;
using UnityEngine;

namespace Ukiyo.Enemy
{
    // Script linked version state machine
    public class AggressiveEnemySM : BaseStateMachine
    {
        
        protected override void Awake()
        {
            base.Awake();
            CurrentState = ScriptableObject.CreateInstance<AggressiveIdleState>();
        }
    }
}