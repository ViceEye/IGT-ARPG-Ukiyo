using UnityEngine;

namespace Ukiyo.Player
{
    public class AttackBehavior : StateMachineBehaviour
    {

        private static readonly int Combo = Animator.StringToHash("Combo");
        private static readonly int One = Animator.StringToHash("1");
        private static readonly int Two = Animator.StringToHash("2");
        private static readonly int Three = Animator.StringToHash("3");

        private int HashCompare(int target)
        {
            if (One == target)
                return 1;
            if (Two == target)
                return 2;
            if (Three == target)
                return 3;
            return -1;
        }
    
        private int GetCombo(Animator animator)
        {
            return (int) animator.GetFloat(Combo);
        }
    
        private void SetCombo(Animator animator, int state)
        {
            animator.SetFloat(Combo, state);
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // If tag is the same (no next combo) or tag is 4 (finished combo) reset attack state
            if (HashCompare(stateInfo.tagHash) == GetCombo(animator) || HashCompare(stateInfo.tagHash) == 3 || HashCompare(stateInfo.tagHash) == -1)
            {
                SetCombo(animator, 0);
            }
        }
        
    }
}
