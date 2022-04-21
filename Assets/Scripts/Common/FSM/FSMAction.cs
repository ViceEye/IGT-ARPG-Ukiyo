using UnityEngine;

namespace Ukiyo.Common.FSM
{
    public abstract class FSMAction : ScriptableObject
    {
        // Called when the default state is loaded and before switching states.
        public virtual void BeforeExecute(BaseStateMachine stateMachine) { }
        
        // Called when state is executing, rely on Update()
        public virtual void Execute(BaseStateMachine stateMachine) { }
        
        // Called when state is executing, rely on FixedUpdate()
        public virtual void FixedExecute(BaseStateMachine stateMachine) { }
        
        // Called when switching states
        public virtual void ExitExecute(BaseStateMachine stateMachine) { }
    }
}