using UnityEngine;
using UnityEngine.Serialization;

namespace Ukiyo.Player
{
    public class AnimationManager : MonoBehaviour
    {
    
        public Animator animator;
        public ThirdPersonController thirdPersonController;
        
        public void Awake()
        {
            GameObject mPlayer = GameObject.FindWithTag("Player");
            if (null != mPlayer)
            {
                animator = mPlayer.GetComponent<Animator>();
                thirdPersonController = mPlayer.GetComponent<ThirdPersonController>();
            }
            else
                Debug.Log("Animator Failed #1");
            if (null == animator)
                Debug.Log("Animator Failed #2");
        }

        #region AnimationParameterIndex
    
        private static readonly int AttackState = Animator.StringToHash("AttackState");
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int XZ = Animator.StringToHash("xz");
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Y = Animator.StringToHash("y");
        private static readonly int Z = Animator.StringToHash("z");

        #endregion

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // Controls Idle and Run
            animator.SetFloat(XZ, new Vector3(horizontal, 0f, vertical).magnitude);
            animator.SetFloat(X, horizontal);
            
            // Controls Jump begin, loop and end.
            animator.SetFloat(Y, thirdPersonController.verticalVelocity.y);
            animator.SetFloat(Z, vertical);
            
            // Controls transition between IdleNRun and Jump
            animator.SetBool(IsIdle, false);

            if (thirdPersonController.IsGrounded)
                animator.SetBool(IsIdle, true);
            else if (!thirdPersonController.IsGrounded && thirdPersonController.verticalVelocity.y > 0)
                animator.SetBool(IsIdle, false);
            
            // todo: Attack State should be accept when animation is nearly finish
            if (Input.GetButtonDown("Fire1"))
                // Combo + 1
                SetAttackState(GetAttackState() + 1);
        }

        #region AttackState

        public bool IsNotAttacking()
        {
            return GetAttackState() == 0;
        }
    
        private int GetAttackState()
        {
            return (int) animator.GetFloat(AttackState);
        }
    
        private void SetAttackState(int state)
        {
            animator.SetFloat(AttackState, state);
        }

        #endregion
    }
}
