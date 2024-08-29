using System;
using System.Collections;
using Npc.Zombie.Base.States.Base;
using UnityEngine;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class ZombieCoolDown :  ZombieState
    {
        public float cooldownTime = 2f; // Cooldown time in seconds

        public override void Enter()
        {
            Debug.Log("Cooling Down State");
            npc.animator.SetFloat(npc.isWalking, 0f); // Ensure walking animation is stopped
            npc.agent.SetDestination(npc.transform.position); // Stop movement

            npc.StartCoroutine(CoolDownCoroutine());
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(cooldownTime);
        
            npc.TransitionToState(npc.chasingState);
        }
        
    }
}