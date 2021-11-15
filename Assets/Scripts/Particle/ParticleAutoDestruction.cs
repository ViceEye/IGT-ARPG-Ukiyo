using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleAutoDestruction : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
 
    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        // Disable all loop animation of particle systems
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;
            main.loop = false;
        }
    }
	
    void Update ()
    {
        bool allStopped = true;
 
        foreach (ParticleSystem ps in particleSystems)
        {
            // Check all particle system finished play and destroy the object
            if (!ps.isStopped)
            {
                allStopped = false;
            }
        }
 
        if (allStopped)
            Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
    }
}
