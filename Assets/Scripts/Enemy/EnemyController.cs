using System.Collections;
using Ukiyo.Common;
using Ukiyo.Common.Object;
using Ukiyo.Serializable.Entity;
using Ukiyo.UI.WorldSpace;
using UnityEngine;
using UnityEngine.AI;

namespace Ukiyo.Enemy
{
    public class EnemyController : BaseController
    {
        [Header("Components")] public NavMeshAgent agent;
        public Animator animator;

        [Header("Runtime Variables")] 
        public GameObject target;
        public Vector3 spawnPosition;
        public float hatredRadius;
        public float spawnStopDistance;
        private Vector3 previousPosition;

        public float currentSpeed;

        [Header("Attributes")] 
        public EnumEntityStatsType entityType;
        public EntityStat enemyStats;
        public HealthBarComponent healthBar;
        
        #region AnimationParameterIndex

        private static readonly int AttackState = Animator.StringToHash("AttackState");
        private static readonly int XZ = Animator.StringToHash("xz");
        private static readonly int Dizzy = Animator.StringToHash("Dizzy");
        private static readonly int Battle = Animator.StringToHash("Battle");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die = Animator.StringToHash("Die");

        #endregion

        protected override void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            spawnPosition = transform.position;
            enemyStats = ObjectPool.Instance.GetStatByType(entityType);
            healthBar = GetComponentInChildren<HealthBarComponent>();
        }

        protected override void Update()
        {
            base.Update();

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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            healthBar.SetHealth(enemyStats.Health, enemyStats.MaxHealth);
        }

        protected override void CheckGrounded()
        {
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out _,
                groundCheckRadius);
        }

        public void SetDizzy(float dizzyTime)
        {
            animator.SetFloat(Dizzy, dizzyTime);
            StartCoroutine(ResetDizzy(dizzyTime));
        }

        IEnumerator ResetDizzy(float dizzyTime)
        {
            yield return new WaitForSeconds(dizzyTime);
            animator.SetFloat(Dizzy, -1.0f);
        }

        public void SetBattle(bool battle)
        {
            animator.SetBool(Battle, battle);
        }

        public void TakeHit(GameObject from)
        {
            animator.SetTrigger(Hit);
        }

        public void PlayDeath(GameObject from)
        {
            animator.SetTrigger(Die);
        }

        public bool Ping()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null && hatredRadius > 0.0f)
            {
                var distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
                if (distance <= hatredRadius)
                {
                    target = player;
                    SetBattle(true);
                    return true;
                }
            }

            target = null;
            SetBattle(false);
            return false;
        }

        public float TargetDistanceToSpawnPoint()
        {
            return Vector3.Distance(target.transform.position, spawnPosition);
        }

        public float SelfDistanceToSpawnPoint()
        {
            return Vector3.Distance(transform.position, spawnPosition);
        }

    }

}