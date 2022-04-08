using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class ScriptAnimator : MonoBehaviour
    {
        public new Animation animation;

        public List<AnimationClip> AnimationClips;

        [Range(0, 6)]
        public int currentPlaying = 0;

        private void Start()
        {
            animation = GetComponent<Animation>();
            foreach (var animationClip in AnimationClips)
            {
                animation.AddClip(animationClip, animationClip.name);
            }
            // animation.AddClip(idle, "idle");
            // animation.AddClip(walk, "walk");
            // animation.AddClip(run, "run");
            // animation.AddClip(attack1, "attack1");
            // animation.AddClip(attack2, "attack2");
            // animation.AddClip(die, "die");
            // animation.AddClip(hit, "hit");
        }

        private void Update()
        {
        }
    }
}