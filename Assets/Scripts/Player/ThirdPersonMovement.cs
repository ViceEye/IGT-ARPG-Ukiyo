using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform groundCheck;
    public Transform skillPoint;
    public LayerMask groundMask;
    public CinemachineFreeLook cinemachineFreeLookPar;
    public SkillManager skillManager;

    [Header("Animator")] 
    public AnimationManager animationManager;
    
    [Header("Attributes")]
    public float speed = 7f;

    public bool enableJump;
    public float jump = 3f;

    public bool enableGravity;
    // Idle drag, keeps character on the floor
    public float baseDrag = -1.5f;
    public float gravity = -9.81f;
    
    // Vertical velocity for jump
    public Vector3 verticalVelocity;
    [SerializeField]
    private bool isGrounded;
    public  bool IsGrounded => isGrounded;
    public float groundCheckRadius = 0.5f;

    [Header("Camera")]
    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;
    public float cameraYAxisSpeed = 2.0f;
    public float cameraXAxisSpeed = 300.0f;

    private void Start()
    {
        animationManager = gameObject.AddComponent<AnimationManager>();
    }

    private void Update()
    {
        CameraLock();
        if (enableJump && animationManager.IsAttacking())
            Jump();
    }
    
    void FixedUpdate()
    {
        if (animationManager.IsAttacking())
        {
            Movement();
        }
        if (enableGravity)
            Gravity();
    }

    private bool rightClicked = false;
    
    void CameraLock()
    {
        float rightClick = Input.GetAxisRaw("Fire2");
        rightClicked = rightClick > 0;
        // If rightClicked unlock camera
        cinemachineFreeLookPar.m_YAxis.m_MaxSpeed = rightClicked ? cameraYAxisSpeed : 0;
        cinemachineFreeLookPar.m_XAxis.m_MaxSpeed = rightClicked ? cameraXAxisSpeed : 0;
        
        // When camera unlocked, scroll could change fov.
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        float fov = cinemachineFreeLookPar.m_Lens.FieldOfView;
        cinemachineFreeLookPar.m_Lens.FieldOfView = rightClicked ? fov - scroll * 5 : fov;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Todo: Bug #1 => Moving forward when jumping and hits a object, drag back effects. Solved.
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

    // Apply gravity when character not on the ground
    void Gravity()
    {
        // Todo: Bug #2 => when collide when a wall is also considered as grounded, which is incorrect, need to change to vertical calculation not sphere calculation
        // Quick Fixed - set the radius smaller
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        // When player is grounded and vertical velocity is less than 0 = descending 
        if (isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = baseDrag;
        }
        
        verticalVelocity.y += gravity * Time.deltaTime * 2;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

}
