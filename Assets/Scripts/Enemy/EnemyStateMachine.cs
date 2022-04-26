using System;
using System.Collections.Generic;
using Enemy.FSM;
using Ukiyo.Common.FSM;
using UnityEngine;

namespace Enemy
{
    // Script linked version state machine
    public class EnemyStateMachine : BaseStateMachine
    {
        public enum ActionEnum
        {
            Update,
            Chase,
            Idle,
            Attack,
        }

        public static Dictionary<ActionEnum, FSMAction> dicActions = new Dictionary<ActionEnum, FSMAction>();

        private void RegisterAllActions()
        {
            dicActions.Add(ActionEnum.Update, ScriptableObject.CreateInstance<IdleAction>());
            dicActions.Add(ActionEnum.Chase, ScriptableObject.CreateInstance<ChaseAction>());
            dicActions.Add(ActionEnum.Idle, ScriptableObject.CreateInstance<IdleAction>());
            dicActions.Add(ActionEnum.Attack, ScriptableObject.CreateInstance<IdleAction>());
        }
        
        protected override void Awake()
        {
            base.Awake();
            RegisterAllActions();
            CurrentState = ScriptableObject.CreateInstance<IdleState>();
        }
    }

    // Script version FSM.State configure Actions and Transition through BeforeExecute()
    # region Enemy States
    public class IdleState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Actions.Add(CreateInstance<IdleAction>());
            Transitions.Add(CreateInstance<DetectingTransition>());
            base.BeforeExecute(stateMachine);
        }
    }
    
    public class ChaseState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
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