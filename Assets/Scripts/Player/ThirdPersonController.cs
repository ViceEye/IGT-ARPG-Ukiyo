using Cinemachine;
using Ukiyo.Common;
using Ukiyo.Skill;
using UnityEngine;

namespace Ukiyo.Player
{
    public class ThirdPersonController : BaseController
    {
        [Header("Components")]
        public Transform cameraTransform;
        public Transform skillPoint;
        public CinemachineFreeLook cinemachineFreeLookPar;
        public SkillManager skillManager;

        [Header("Animator")] 
        public AnimationManager animationManager;

        [Header("Camera")]
        public float turnSmoothTime = 0.2f;
        private float turnSmoothVelocity;
        public float cameraYAxisSpeed = 2.0f;
        public float cameraXAxisSpeed = 300.0f;

        protected override void Start()
        {
            animationManager = gameObject.AddComponent<AnimationManager>();
            cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            cinemachineFreeLookPar = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineFreeLook>();
        }

        protected override void Update()
        {
            CameraLock();
            if (enableJump && animationManager.IsNotAttacking())
                Jump();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (animationManager.IsNotAttacking())
            {
                Movement();
            }
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
            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
            }
        }

        void Movement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                // Direction of movement rotation
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                // Smoothed direction of movement rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                    turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Movement
                Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
                controller.Move(moveDirection * speed * Time.deltaTime);
            }
        }

    }
}
