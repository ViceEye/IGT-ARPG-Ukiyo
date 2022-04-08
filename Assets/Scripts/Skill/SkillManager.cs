using System.Collections;
using Ukiyo.Player;
using UnityEngine;

namespace Ukiyo.Skill
{
    public class SkillManager : MonoBehaviour
    {
        public Transform position;
        public GameObject[] prefabs;

        public void PlaySkill(int number)
        {
            // switch (number)
            // {
            //     case 1:
            //         Instantiate(prefabs[0], position.position,
            //             new Quaternion(-0.132389709f, -0.0443884619f, -0.971372426f, 0.192193419f), null);
            //         break;
            //     case 2:
            //         Instantiate(prefabs[1], position.position,
            //             new Quaternion(0.0341425501f,-0.328095704f,0.166466668f,0.929234266f), null);
            //         break;
            //     case 3:
            //         Instantiate(prefabs[2], position.position,
            //             new Quaternion(0.241348505f,-0.202374473f,0.91386658f,0.256209761f), null);
            //         break;
            //     case 4:
            //         Instantiate(prefabs[3], position.position,
            //             new Quaternion(0.309731603f,0.309731513f,0.635662138f,-0.635662138f), null);
            //         break;
            // }
        }

        public void DoAttack()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in gameObjects)
            {
                bool attack = CollisionDetector.Instance.FanShapedCheck(transform, obj.transform, 160, 2);
                Debug.Log(attack);
                if (attack)
                {
                    //obj.GetComponent<MeshRenderer>().materials[0].SetColor("_AlbedoTint", Color.red);
                    //StartCoroutine(ResetMaterial(0.2f, obj));

                    EnemyController ec = obj.GetComponent<EnemyController>();
                    if (ec != null)
                    {
                        ec.TakeHit(gameObject);
                    }
                }
            }
        }

        IEnumerator ResetMaterial(float time, GameObject obj)
        {
            yield return new WaitForSeconds(time);
            obj.GetComponent<MeshRenderer>().materials[0].SetColor("_AlbedoTint", Color.white);
        }

    }
}
