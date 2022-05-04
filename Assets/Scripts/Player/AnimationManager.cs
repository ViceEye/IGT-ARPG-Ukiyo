using Ukiyo.Framework;
using UnityEngine;

namespace Ukiyo.Player
{
    public class AnimationManager : MonoBehaviour, IAnimatorListener
    {
    
        public Animator animator;
        public ThirdPersonController thirdPersonController;
        [SerializeField]
        private bool acceptingCombo;
        
        #region AnimationParameterIndex
    
        private static readonly int Combo = Animator.StringToHash("Combo");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int XZ = Animator.StringToHash("xz");
        private static readonly int X = Animator.StringToHash("x");
        private static readonly int Y = Animator.StringToHash("y");
        private static readonly int Z = Animator.StringToHash("z");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Equip = Animator.StringToHash("Equip");
        private static readonly int DoEquip = Animator.StringToHash("DoEquip");

        #endregion
        
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

        // WASD movement, LShift run, Q equip/un-equip, E inventory, C roll
        public void OnUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // Magnitude of absolute movement speed
            animator.SetFloat(XZ, new Vector3(horizontal, 0f, vertical).magnitude);
            
            // Velocity of the three axes
            animator.SetFloat(X, horizontal);
            animator.SetFloat(Y, thirdPersonController.verticalVelocity.y);
            animator.SetFloat(Z, vertical);

            // Is running
            animator.SetBool(Run, Input.GetKey(KeyCode.LeftShift));
            
            // Equip/Un-Equip
            if (Input.GetKeyUp(KeyCode.Q))
                TryEquip();

            if (thirdPersonController.IsGrounded)
                animator.SetBool(IsGrounded, true);
            else if (!thirdPersonController.IsGrounded && thirdPersonController.verticalVelocity.y > 0)
                animator.SetBool(IsGrounded, false);
            
            TrySetCombo();
        }

        public void OnAnimatorBehaviourMessage(string message, object value)
        {
            switch (message)
            {
                case "BeginCombo":
                {
                    SetCombo(1);
                    break;
                }
                case "AcceptingCombo":
                {
                    acceptingCombo = true;
                    break;
                }
                case "EndingCombo":
                {
                    acceptingCombo = false;
                    break;
                }
                case "EndCombo":
                {
                    acceptingCombo = false;
                    SetCombo(0);
                    break;
                }
                case "SwitchEquip":
                {
                    thirdPersonController.DoEquip();
                    thirdPersonController.allowMovement = true;
                    animator.SetBool(Equip, thirdPersonController.isEquipped);
                    break;
                }
            }
        }

        public void DoJump()
        {
            animator.SetTrigger(Jump);
        }

        public bool IsEquip()
        {
            return animator.GetBool(Equip);
        }
        
        public void TryEquip()
        {
            if (!animator.GetBool(Run))
            {
                thirdPersonController.allowMovement = false;
                animator.SetTrigger(DoEquip);
            }
        }

        #region Combo

        public void TrySetCombo()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (GetCombo() == 0 && !acceptingCombo && IsEquip())
                {
                    SetCombo(GetCombo() + 1);
                    return;
                }
                if (acceptingCombo)
                {
                    SetCombo(GetCombo() + 1);
                    acceptingCombo = false;
                }
            }
        }
        
        public bool IsNotAttacking()
        {
            return GetCombo() == 0;
        }
    
        private int GetCombo()
        {
            return (int) animator.GetFloat(Combo);
        }
    
        private void SetCombo(int state)
        {
            animator.SetFloat(Combo, state);
        }

        #endregion
    }
}
