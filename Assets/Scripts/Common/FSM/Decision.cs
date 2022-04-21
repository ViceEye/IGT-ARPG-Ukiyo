using UnityEngine;

namespace Ukiyo.Common.FSM
{
    /// <summary>
    /// Base class for a Decision
    ///
    /// If there are many variables to determine, encapsulate a conditional object for the FSM.
    /// Will be much faster and easier to create a decision object.
    /// </summary>
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine state);
    }
}