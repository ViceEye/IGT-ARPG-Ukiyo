using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    
    #region AttackState

    private static readonly int AttackState = Animator.StringToHash("AttackState");
    private static readonly int One = Animator.StringToHash("1");
    private static readonly int Two = Animator.StringToHash("2");
    private static readonly int Three = Animator.StringToHash("3");
    private static readonly int Four = Animator.StringToHash("4");

    private int HashCompare(int target)
    {
        if (One == target)
            return 1;
        if (Two == target)
            return 2;
        if (Three == target)
            return 3;
        if (Four == target)
            return 4;
        return -1;
    }
    
    private int GetAttackState(Animator animator)
    {
        return (int) animator.GetFloat(AttackState);
    }
    
    private void SetAttackState(Animator animator, int state)
    {
        animator.SetFloat(AttackState, (float) state);
    }

    #endregion

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If tag is the same (no next combo) or tag is 4 (finished combo) reset attack state
        if (HashCompare(stateInfo.tagHash) == GetAttackState(animator) || HashCompare(stateInfo.tagHash) == 4)
        {
            SetAttackState(animator, 0);
        }
    }
    
    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // // OnStateMachineEnter is called when entering a state machine via its Entry Node
    // public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    // {
    //     Debug.Log("OnStateMachineEnter");
    // }
    //
    // // OnStateMachineExit is called when exiting a state machine via its Exit Node
    // public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    // {
    //     Debug.Log("OnStateMachineExit");
    // }
}
