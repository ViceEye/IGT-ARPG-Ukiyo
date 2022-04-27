using Cinemachine;
using Ukiyo.Common;
using Ukiyo.Framework;
using Ukiyo.Skill;
using UnityEngine;

namespace Ukiyo.Player
{
    public class ThirdPersonController : BaseController, IAnimatorListener
    {
        [Header("Components")]
        public Transform cameraTransform;
        public CinemachineFreeLook cinemachineFreeLookPar;
        public SkillManager skillManager;

        [Header("Animator")] 
        public AnimationManager animationManager;

        [Header("Camera")] 
        public Transform lookingAt;
        public float turnSmoothTime = 0.2f;
        private float turnSmoothVelocity;
        public float cameraYAxisSpeed = 2.0f;
        public float cameraXAxisSpeed = 300.0f;

        [Header("Attributes")]
        public float walkSpeed = 6f;
        public float runSpeed = 10f;
        [SerializeField]
        protected float targetAngle;
        public float TargetAngle => targetAngle;
        
        [Header("Sword")] 
        public bool isEquipped = true;
        public GameObject backSword;
        public GameObject handSword;

        public bool allowMovement;
        
        protected override void Start()
        {
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            cinemachineFreeLookPar = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineFreeLook>();
            cinemachineFreeLookPar.Follow = transform;
            cinemachineFreeLookPar.LookAt = lookingAt;
            skillManager = gameObject.GetComponent<SkillManager>();
            if (skillManager == null)
                gameObject.AddComponent<SkillManager>();
            animationManager = gameObject.AddComponent<AnimationManager>();
        }

        protected override void Update()
        {
            base.Update();
            CameraLock();
            if (enableJump && animationManager.IsNotAttacking())
                Jump();
            animationManager.OnUpdate();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (animationManager.IsNotAttacking())
                Movement();
        }
        
        void CameraLock()
        {
            float rightClick = Input.GetAxisRaw("Fire2");
            bool rightClicked = rightClick > 0;
            // If rightClicked unlock camera
            cinemachineFreeLookPar.m_YAxis.m_MaxSpeed = rightClicked ? cameraYAxisSpeed : 0;
            cinemachineFreeLookPar.m_XAxis.m_MaxSpeed = rightClicked ? cameraXAxisSpeed : 0;
        
            // When camera unlocked, scroll could change fov.
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            float fov = cinemachineFreeLookPar.m_Lens.FieldOfView;
            cinemachineFreeLookPar.m_Lens.FieldOfView = rightClicked ? fov - scroll * 5 : fov;
        }

        protected override void Jump()
        {
            // No movement allowed before the spawn animation is finished
            if (!allowMovement) return;
            
            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                animationManager.DoJump();
                verticalVelocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
            }
        }

        void Movement()
        {
            // No movement allowed before the spawn animation is finished
            if (!allowMovement) return;
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            bool shift = Input.GetKey(KeyCode.LeftShift);

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                // Direction of movement rotation
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float camTargetAngle = targetAngle + cameraTransform.eulerAngles.y;
                // Smoothed direction of movement rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, camTargetAngle, ref turnSmoothVelocity,
                    turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Movement
                Vector3 moveDirection = (Quaternion.Euler(0f, camTargetAngle, 0f) * Vector3.forward).normalized;
                float speed = shift ? runSpeed : walkSpeed;
                controller.Move(moveDirection * speed * Time.deltaTime);
            }
        }

        protected override void CheckGrounded()
        {
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out _, groundCheckRadius);
        }

        public void DoEquip()
        {
            isEquipped = !isEquipped;
            if (isEquipped)
            {
                handSword.SetActive(true);
                backSword.SetActive(false);
            }
            else
            {
                handSword.SetActive(false);
                backSword.SetActive(true);
            }
        }

        public void OnAnimatorBehaviourMessage(string message, object value)
        {
            switch (message)
            {
                case "FinishSpawn":
                {
                    allowMovement = true;
                    break;
                }
            }
        }
    }
}
