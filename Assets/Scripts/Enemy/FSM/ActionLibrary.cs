using System;
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

    public class AttackAction : FSMAction
    {
        public override void FixedExecute(BaseStateMachine stateMachine)
        {
            var enemyController = stateMachine.GetComponent<EnemyController>();

            var distance = Vector3.Distance(enemyController.target.transform.position, enemyController.transform.position);
            var targetAttackTime = enemyController.lastAttackTime + (long) (enemyController.attackCooldown * 1000);
            var nowTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            
            if (distance <= enemyController.attackRadius && targetAttackTime <= nowTime && enemyController.IsTargetAlive())
                enemyController.PlayAttack(1);
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