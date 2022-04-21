using UnityEngine;

namespace Ukiyo.Common.FSM
{
    /// <summary>
    /// Decide to determine which state will be activated
    /// </summary>
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public virtual void BeforeExecute(BaseStateMachine stateMachine) { }
        
        public virtual void Execute(BaseStateMachine stateMachine)
        {
            if (Decision.Decide(stateMachine) && !(TrueState is RemainInState))
            {
                stateMachine.CurrentState.ExitExecute(stateMachine);
                TrueState.BeforeExecute(stateMachine);
                stateMachine.CurrentState = TrueState;
            }
            else if(!(FalseState is RemainInState))
            {
                FalseState.BeforeExecute(stateMachine);
                stateMachine.CurrentState = FalseState;
            }
        }
        
    }
}