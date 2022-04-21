using Ukiyo.Common.FSM;
using UnityEngine;

namespace Enemy.FSM
{
    [CreateAssetMenu(menuName = "FSM/Action/Idle")]
    public class IdleAction : FSMAction
    {

        public override void FixedExecute(BaseStateMachine stateMachine)
        {
        }
    }
}