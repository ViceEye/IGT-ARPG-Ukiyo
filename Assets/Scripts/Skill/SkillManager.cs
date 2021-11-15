using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject[] prefabs;

    public void testSkill(Transform position)
    {
        Instantiate(prefabs[0], position.position, position.rotation, null);
    }
    
}
