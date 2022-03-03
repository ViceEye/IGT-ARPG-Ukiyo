using UnityEngine;

namespace Ukiyo.Player
{
    public class AnimationManager : MonoBehaviour
    {
    
        public Animator animator;
        public ThirdPersonMovement thirdPersonMovement;
    
        public void Awake()
        {
            GameObject mPlayer = GameObject.FindWithTag("Player");
            if (null != mPlayer)
            {
                animator = mPlayer.GetComponent<Animator>();
                thirdPersonMovement = mPlayer.GetComponent<ThirdPersonMovement>();
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
            animator.SetFloat(Y, thirdPersonMovement.verticalVelocity.y);
            animator.SetFloat(Z, vertical);
            
            // Controls transition between IdleNRun and Jump
            animator.SetBool(IsIdle, false);

            if (thirdPersonMovement.IsGrounded)
                animator.SetBool(IsIdle, true);
            else if (!thirdPersonMovement.IsGrounded && thirdPersonMovement.verticalVelocity.y > 0)
                animator.SetBool(IsIdle, false);
            
            // todo: Attack State should be accept after animation is finish
            if (Input.GetButtonDown("Fire1"))
                // Combo + 1
                SetAttackState(GetAttackState() + 1);
        }

        #region AttackState

        public bool IsAttacking()
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
