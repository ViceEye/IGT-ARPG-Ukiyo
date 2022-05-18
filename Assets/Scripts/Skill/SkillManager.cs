using System.Collections;
using Ukiyo.Enemy;
using Ukiyo.Framework;
using Ukiyo.Player;
using UnityEngine;

namespace Ukiyo.Skill
{
    public class SkillManager : MonoBehaviour, IAnimatorListener
    {
        public ThirdPersonController _controller;
        public Transform selfPosition;
        public Transform forwardPosition;
        public GameObject[] prefabs;

        public void PlaySkill(int number, bool self = false)
        {
            if (prefabs.Length <= number + 1)
            {
                GameObject go = Instantiate(prefabs[number], transform);
                go.transform.position = selfPosition.position;
                go.transform.rotation = selfPosition.rotation;
                SkillAttack();
            }
        }

        public void SkillAttack()
        {
            // Find all enemies
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in gameObjects)
            {
                // If enemy is within range
                bool attack = CollisionDetector.Instance.FanShapedCheck(transform, obj.transform, 80, 18);
                if (attack)
                {
                    EnemyController enemy = obj.GetComponent<EnemyController>();
                    if (enemy != null)
                        // Skill deals double damage
                        enemy.TakeHit(_controller.gameObject, _controller.GetDamage() * 2);
                }
            }
        }

        public void DoAttack()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in gameObjects)
            {
                bool attack = CollisionDetector.Instance.FanShapedCheck(transform, obj.transform, 160, 4);
                if (attack)
                {
                    EnemyController enemy = obj.GetComponent<EnemyController>();
                    if (enemy != null)
                        enemy.TakeHit(_controller.gameObject, _controller.GetDamage());
                }
            }
        }

        // Damage flash effect
        void SetMaterial(GameObject obj)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.materials[0].SetColor("_AlbedoTint", Color.red);
                StartCoroutine(ResetMaterial(0.2f, obj));
            }
        }

        IEnumerator ResetMaterial(float time, GameObject obj)
        {
            yield return new WaitForSeconds(time);
            obj.GetComponent<MeshRenderer>().materials[0].SetColor("_AlbedoTint", Color.white);
        }

        public void OnAnimatorBehaviourMessage(string message, object value)
        {
            Debug.Log(message);
            switch (message)
            {
                case "DoAttack":
                {
                    DoAttack();
                    break;
                }
                case "PlaySkill":
                {
                    PlaySkill((int) value);
                    break;
                }
                case "PlayTrail":
                {
                    _controller.activeTrail = true;
                    break;
                }
                case "EndTrail":
                {
                    _controller.activeTrail = false;
                    break;
                }
            }
        }
    }
}
