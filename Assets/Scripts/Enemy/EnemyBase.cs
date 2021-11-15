using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemy
{
    [Header("Components")]
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    
    [Header("Attributes")]
    public int health;
    public float speed = 7f;

    public bool enableJump;
    public float jump = 3f;

    public bool enableGravity;
    public float baseDrag = -1.5f;
    public float gravity = -9.81f;
    
    public Vector3 velocity;
    [SerializeField]
    private bool isGrounded;
    public float groundCheckRadius = 0.5f;

    [Header("Status")] 
    public IEnemy.AiState state;
    
    private void Update()
    {
        
    }
    
    
    void FixedUpdate()
    {
        Movement();
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
    }
    
    void Movement()
    {
        if (enableGravity)
            Gravity();
        
        
    }

    void Gravity()
    {
        // Apply gravity when character not on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = baseDrag;

        velocity.y += gravity * Time.deltaTime * 2;
        controller.Move(velocity * Time.deltaTime);
    }

    public void KillEnemy()
    {
        
    }

    public void DamageEnemy(float dmg)
    {
        
    }
}
