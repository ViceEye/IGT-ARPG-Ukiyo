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
            if (prefabs.Length <= number)
            {
                Vector3 direction = Quaternion.Euler(0f, _controller.TargetAngle, 0f) * Vector3.forward;
                Instantiate(prefabs[number], self ? selfPosition.position : forwardPosition.position,
                    Quaternion.Euler(direction), null);
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
                    Debug.Log(enemy);
                    if (enemy != null)
                    {
                        enemy.TakeHit(_controller.gameObject, _controller.GetDamage());
                    }
                }
            }
        }

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
            switch (message)
            {
                case "DoAttack":
                {
                    DoAttack();
                    break;
                }
                case "PlayerSkill":
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
