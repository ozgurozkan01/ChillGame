using System;
using System.Collections;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class DeathState : ZombieState
    {
        public override void Enter()
        {
            Debug.Log("Entering Death State");
            npc.agent.SetDestination(npc.transform.position);
            npc.animator.SetTrigger(npc.death);
            
            npc.StopAllCoroutines();
            ScaleYAndResetPosition();
            npc.StartCoroutine(npc.WaitForAnimationToFinishCallAction(KillZombie));
        }

        private void KillZombie()
        {
            Object.Destroy(npc.agent);
            Object.Destroy(npc.animator);
            Object.Destroy(npc);
        }
        
        private void ScaleYAndResetPosition()
        {
            var obj = npc.holyCube;
            var targetScale = new Vector3(1, 2, 1); // Set the target scale with Y = 2
            var transform = obj.transform;

            transform.localScale = targetScale;
            obj.gameObject.SetActive(true);
        }

    }
}