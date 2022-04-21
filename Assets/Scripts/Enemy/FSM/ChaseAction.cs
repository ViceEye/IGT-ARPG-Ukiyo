using Ukiyo.Common.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.FSM
{
    [CreateAssetMenu(menuName = "FSM/Action/Chase")]
    public class ChaseAction : FSMAction
    {
        public override void Execute(BaseStateMachine stateMachine)
        {
        }

        public override void FixedExecute(BaseStateMachine stateMachine)
        {
            Debug.Log("Chasing");
            var navAgent = stateMachine.GetComponent<NavMeshAgent>();
            var sensor = stateMachine.GetComponent<EnemySightSensor>();

            navAgent.SetDestination(sensor._player.transform.position);
        }
    }
}