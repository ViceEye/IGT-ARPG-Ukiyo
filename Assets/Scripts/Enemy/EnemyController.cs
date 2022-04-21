using System;
using System.Collections;
using Ukiyo.Common;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : BaseController
{
    [Header("Components")] 
    public NavMeshAgent agent;
    public Animator animator;
    
    private Vector3 previousPosition;

    public GameObject followTarget;
    public float currentSpeed;
    
    private static readonly int XZ = Animator.StringToHash("xz");
    private static readonly int Dizzy = Animator.StringToHash("Dizzy");
    private static readonly int Hit = Animator.StringToHash("Hit");

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        // Move Speed Calculation
        var position = transform.position;
        Vector3 currentPosition = position - previousPosition;
        currentSpeed = currentPosition.magnitude / Time.deltaTime;
        previousPosition = position;
        
        // Controls IdleNRun Animate State
        animator.SetFloat(XZ, currentSpeed);
        // If self is in dizzy state, pause the navigation
        agent.isStopped = animator.GetFloat(Dizzy) > -1.0f;
    }
    
    public void TakeHit(GameObject from)
    {
        followTarget = from;
        animator.SetTrigger(Hit);
        Debug.Log("Taking hit");
    }

    public void SetDizzy(float dizzyTime)
    {
        animator.SetFloat(Dizzy, 1.0f);
        StartCoroutine(ResetDizzy(1.0f));
    }

    IEnumerator ResetDizzy(float dizzyTime)
    {
        yield return new WaitForSeconds(dizzyTime);
        animator.SetFloat(Dizzy, -1.0f);
    }
    
}