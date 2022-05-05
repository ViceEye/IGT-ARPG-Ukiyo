using Ukiyo.Common.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Enemy.FSM
{
    [CreateAssetMenu(menuName = "FSM/Action/Chase")]
    public class ChaseAction : FSMAction
    {
        public override void FixedExecute(BaseStateMachine stateMachine)
        {
            var navAgent = stateMachine.GetComponent<NavMeshAgent>();
            var enemyController = stateMachine.GetComponent<EnemyController>();

            navAgent.stoppingDistance = 3.0f;
            navAgent.SetDestination(enemyController.target.transform.position);
        }
    }

    public class BackToSpawnAction : FSMAction
    {
        public override void FixedExecute(BaseStateMachine stateMachine)
        {
            var navAgent = stateMachine.GetComponent<NavMeshAgent>();
            var enemyController = stateMachine.GetComponent<EnemyController>();

            navAgent.stoppingDistance = 0.0f;
            navAgent.SetDestination(enemyController.spawnPosition);
        }
    }
    
    [CreateAssetMenu(menuName = "FSM/Action/Idle")]
    public class IdleAction : FSMAction { }
    
    
}