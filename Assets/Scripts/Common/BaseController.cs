using UnityEngine;

namespace Ukiyo.Common
{
    // Base Controller supports Basic Physics about Character Controller and EnemyController
    public abstract class BaseController : MonoBehaviour
    {

        [Header("Components")]
        public CharacterController controller;
        public Transform groundCheck;
        public LayerMask groundMask;
        
        [Header("Attributes")]
        public bool enableJump;
        public float jump = 3f;

        public bool enableGravity;
        // Idle drag, keeps character on the floor
        public float baseDrag = -1.5f;
        public float gravity = -9.81f;
    
        // Vertical velocity for jump
        public Vector3 verticalVelocity;
        [SerializeField] protected bool isGrounded;
        public  bool IsGrounded => isGrounded;
        public float groundCheckRadius = 0.4f;

        // Start is called before the first frame update
        protected virtual void Start()
        {
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            CheckGrounded();
        }

        protected virtual void FixedUpdate()
        {
            if (enableGravity)
                Gravity();
        }
        
        protected virtual void Jump()
        {
            verticalVelocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
        }

        protected virtual void Gravity()
        {
            // When player is grounded and vertical velocity is less than 0 = descending 
            if (isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = baseDrag;
            }

            verticalVelocity.y += gravity * Time.deltaTime * 2;
            controller.Move(verticalVelocity * Time.deltaTime);
        }
        
        protected virtual void CheckGrounded()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        }
    }
}