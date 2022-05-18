using Ukiyo.Framework;
using UnityEngine;

namespace Ukiyo.Enemy
{
    class EnemyAnimationListener : MonoBehaviour, IAnimatorListener
    {
        public EnemyController enemyController;

        public void OnAnimatorBehaviourMessage(string message, object value)
        {
            switch (message)
            {
                case "EndAttack":
                {
                    enemyController.ResetAttack();
                    break;
                }
                case "DoAttack":
                {
                    enemyController.DamageInFront();
                    break;
                }
                case "MonsterDied":
                {
                    enemyController.InvokeDeathEvent();
                    GameObject go = enemyController.gameObject;
                    go.SetActive(false);
                    Destroy(go, 1.5f);
                    break;
                }
            }
        }
    }
}