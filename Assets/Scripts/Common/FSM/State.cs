using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ukiyo.Common.FSM
{
    [CreateAssetMenu(menuName = "FSM/State")]
    public class State : BaseState
    {
        /// <summary>
        /// Actions that will be performed when this state is active
        /// </summary>
        public List<FSMAction> Actions = new List<FSMAction>();
        /// <summary>
        /// Conditions that will be checked when this state is active
        /// </summary>
        public List<Transition> Transitions = new List<Transition>();

        // Scripting friendly API
        public void AddTransition(Func<bool> transitionCondition, BaseState trueState, BaseState falseState)
        {
            Transition transition = CreateInstance<Transition>();
            transition.Func = transitionCondition;
            transition.TrueState = trueState;
            transition.FalseState = falseState;
            
            Transitions.Add(transition);
        }
        
        // Designer friendly API
        public void AddTransition(Decision transitionCondition, BaseState trueState, BaseState falseState)
        {
            Transition transition = CreateInstance<Transition>();
            transition.Decision = transitionCondition;
            transition.TrueState = trueState;
            transition.FalseState = falseState;
            
            Transitions.Add(transition);
        }

        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            isExit = false;
            
            foreach (var fsmAction in Actions)
                fsmAction.BeforeExecute(stateMachine);
            
            foreach (var fsmTransition in Transitions)
                fsmTransition.BeforeExecute(stateMachine);
        }

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (isExit) return;
            
            foreach (var action in Actions)
                action.Execute(stateMachine);

            foreach(var transition in Transitions)
                transition.Execute(stateMachine);
        }

        public override void FixedExecute(BaseStateMachine stateMachine)
        {
            foreach (var action in Actions)
                action.FixedExecute(stateMachine);
        }

        public override void ExitExecute(BaseStateMachine stateMachine)
        {
            isExit = true;
            
            foreach (var fsmAction in Actions)
                fsmAction.ExitExecute(stateMachine);
        }
    }
}