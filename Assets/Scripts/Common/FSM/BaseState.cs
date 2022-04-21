using UnityEngine;

namespace Ukiyo.Common.FSM
{
    public class BaseState : ScriptableObject
    {
        // Prerequisites for Execute() and ExitExecute()
        protected bool isExit;
        
        // Called when the default state is loaded and before switching states.
        public virtual void BeforeExecute(BaseStateMachine stateMachine) { }
        
        // Called when FSM is executing, rely on Update()
        public virtual void Execute(BaseStateMachine stateMachine) { }
        
        // Called when FSM is executing, rely on FixedUpdate()
        public virtual void FixedExecute(BaseStateMachine stateMachine) { }
        
        // Called when switching states
        public virtual void ExitExecute(BaseStateMachine stateMachine) { }
    }
}