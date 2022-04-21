using Enemy.FSM;
using Ukiyo.Common.FSM;
using UnityEngine;

namespace Enemy
{
    // Script linked version state machine
    public class EnemyStateMachine : BaseStateMachine
    {
        protected override void Awake()
        {
            base.Awake();
            CurrentState = ScriptableObject.CreateInstance<IdleState>();
        }
    }

    // Script version FSM.State configure Actions and Transition through BeforeExecute()
    # region Enemy States
    public class IdleState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Debug.Log("Outer");
            Actions.Add(CreateInstance<IdleAction>());
            Transitions.Add(CreateInstance<DetectingTransition>());
            base.BeforeExecute(stateMachine);
        }
    }
    
    public class ChaseState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Debug.Log("Chase");
            Actions.Add(CreateInstance<ChaseAction>());
            base.BeforeExecute(stateMachine);
        }
    }
    # endregion
    
    
    # region State Transition
    public class DetectingTransition : Transition
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Decision = CreateInstance<InSightDecision>();
            TrueState = CreateInstance<ChaseState>();
            FalseState = CreateInstance<RemainInState>();
        }
    }
    # endregion
}