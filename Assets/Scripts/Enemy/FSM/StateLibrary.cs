using Ukiyo.Common.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Enemy.FSM
{
    // Script version FSM.State configure Actions and Transition through BeforeExecute() 
    # region Enemy States
    public class AggressiveIdleState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Actions.Clear();
            Transitions.Clear();
            
            var enemyController = stateMachine.GetComponent<EnemyController>();
            
            Actions.Add(stateMachine.GetAction<IdleAction>());
            
            // Too complex to setup, so simplified
            // Transitions.Add(stateMachine.GetTransition<DetectingTransition>());
            AddTransition(() => enemyController.Ping(),
                stateMachine.GetState<ChaseState>(),
                stateMachine.GetState<RemainInState>());
            
            base.BeforeExecute(stateMachine);
        }
    }

    public class PassiveIdleState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Actions.Clear();
            Transitions.Clear();

            var enemyController = stateMachine.GetComponent<EnemyController>();

            Actions.Add(stateMachine.GetAction<IdleAction>());

            // Too complex to setup, so simplified
            // Transitions.Add(stateMachine.GetTransition<HurtingTransition>());
            AddTransition(() => enemyController.target != null,
                stateMachine.GetState<ChaseState>(),
                stateMachine.GetState<RemainInState>());

            base.BeforeExecute(stateMachine);
        }
    }

    public class ChaseState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Actions.Clear();
            Transitions.Clear();

            var enemyController = stateMachine.GetComponent<EnemyController>();

            Actions.Add(stateMachine.GetAction<ChaseAction>());

            // Too complex to setup, so simplified
            // Transitions.Add(stateMachine.GetTransition<ChaseTransition>());
            AddTransition(() => enemyController.TargetDistanceToSpawnPoint() <= enemyController.hatredRadius,
                stateMachine.GetState<RemainInState>(),
                stateMachine.GetState<BackToSpawnState>());
            
            base.BeforeExecute(stateMachine);
        }
    }

    public class BackToSpawnState : State
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Actions.Clear();
            Transitions.Clear();

            var enemyController = stateMachine.GetComponent<EnemyController>();
            
            Actions.Add(stateMachine.GetAction<BackToSpawnAction>());

            //Determine if the State should be Aggressive or Passive
            BaseState idleState = stateMachine is AggressiveEnemySM
                ? stateMachine.GetState<AggressiveIdleState>()
                : stateMachine.GetState<PassiveIdleState>();
            
            AddTransition(() => enemyController.SelfDistanceToSpawnPoint() <= enemyController.spawnStopDistance,
                idleState,
                stateMachine.GetState<RemainInState>());
            
            base.BeforeExecute(stateMachine);
        }
    }
    # endregion
    
    
    
    // Too complex to setup for only scripting
    // Updated Func<bool> for running decision for a transition
    # region State Transition
    public class DetectingTransition : Transition
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Decision = stateMachine.GetDecision<InSightDecision>();
            TrueState = stateMachine.GetState<ChaseState>();
            FalseState = stateMachine.GetState<RemainInState>();
        }
    }
    
    public class HurtingTransition : Transition
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Decision = stateMachine.GetDecision<HurtDecision>();
            TrueState = stateMachine.GetState<ChaseState>();
            FalseState = stateMachine.GetState<RemainInState>();
        }
    }
    
    public class ChaseTransition : Transition
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Decision = stateMachine.GetDecision<ChaseDecision>();
            TrueState = stateMachine.GetState<RemainInState>();
            FalseState = stateMachine.GetState<BackToSpawnState>();
        }
    }
    
    public class ReachTransition : Transition
    {
        public override void BeforeExecute(BaseStateMachine stateMachine)
        {
            Decision = stateMachine.GetDecision<ReachDecision>();
            TrueState = stateMachine.GetState<RemainInState>();
            FalseState = stateMachine.GetState<BackToSpawnState>();
        }
    }
    # endregion
}