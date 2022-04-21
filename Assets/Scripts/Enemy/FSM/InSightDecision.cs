using Ukiyo.Common.FSM;
using UnityEngine;

namespace Enemy.FSM
{
    [CreateAssetMenu(menuName = "FSM/Decisions/In Line Of Sight")]
    public class InSightDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var sensor = stateMachine.GetComponent<EnemySightSensor>();
            return sensor.Ping();
        }
    }
}