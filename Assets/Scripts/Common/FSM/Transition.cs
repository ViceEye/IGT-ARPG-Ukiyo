using System;
using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Common.FSM
{
    /// <summary>
    /// Decide to determine which state will be activated
    /// </summary>
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public class Transition : ScriptableObject
    {
        // Scripting friendly
        public Func<bool> Func;
        // Inspector friendly
        public Decision Decision;
        
        public BaseState TrueState;
        public BaseState FalseState;

        public virtual void BeforeExecute(BaseStateMachine stateMachine) { }
        
        public virtual void Execute(BaseStateMachine stateMachine)
        {
            // Debug.Log(stateMachine.CurrentState + " => " + Func());
            // Debug.Log(TrueState);
            // Debug.Log(FalseState);
            
            // Run Inspector friendly Decision
            if (Decision != null)
            {
                if (Decision.Decide(stateMachine) && !(TrueState is RemainInState))
                {
                    stateMachine.CurrentState.ExitExecute(stateMachine);
                    TrueState.BeforeExecute(stateMachine);
                    stateMachine.CurrentState = TrueState;
                }
                else if(!Decision.Decide(stateMachine) && !(FalseState is RemainInState))
                {
                    FalseState.BeforeExecute(stateMachine);
                    stateMachine.CurrentState = FalseState;
                }
            }
            // Run Scripting friendly Func
            else
            {
                Debug.Log("Going To True?");
                Debug.Log(Func() && !(TrueState is RemainInState));
                Debug.Log("Going To False?");
                Debug.Log(!Func() && !(FalseState is RemainInState));
                Debug.Log("===========================");
                if (Func() && !(TrueState is RemainInState))
                {
                    stateMachine.CurrentState.ExitExecute(stateMachine);
                    TrueState.BeforeExecute(stateMachine);
                    stateMachine.CurrentState = TrueState;
                }
                else if(!Func() && !(FalseState is RemainInState))
                {
                    FalseState.BeforeExecute(stateMachine);
                    stateMachine.CurrentState = FalseState;
                }
            }
        }
        
    }
}