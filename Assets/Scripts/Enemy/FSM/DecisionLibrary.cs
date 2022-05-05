using Ukiyo.Common.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Enemy.FSM
{
    // Inspector friendly
    [CreateAssetMenu(menuName = "FSM/Decisions/In Line Of Sight")]
    public class InSightDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var enemyController = stateMachine.GetComponent<EnemyController>();
            
            return enemyController.Ping();
        }
    }
    
    public class HurtDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var enemyController = stateMachine.GetComponent<EnemyController>();
            
            return enemyController.target != null;
        }
    }
    
    public class ChaseDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var enemyController = stateMachine.GetComponent<EnemyController>();
            
            return Vector3.Distance(enemyController.target.transform.position,
                enemyController.spawnPosition) <= enemyController.hatredRadius;
        }
    }

    public class ReachDecision : Decision
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var enemyController = stateMachine.GetComponent<EnemyController>();
            var navAgent = stateMachine.GetComponent<NavMeshAgent>();
            
            return Vector3.Distance(enemyController.transform.position,
                navAgent.destination) <= 0.5f;
        }
    }
    
}