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
        public float duration = 2f; // Duration over which the scale will change

        public override void Enter()
        {
            Debug.Log("Entering Death State");
            npc.agent.SetDestination(npc.transform.position);
            npc.animator.SetTrigger(npc.death);
            
            npc.StopAllCoroutines();
            npc.StartCoroutine(ScaleYAndResetPosition(npc.holyCube, 2f));
            npc.StartCoroutine(npc.WaitForAnimationToFinishCallAction(KillZombie));
        }

        private void KillZombie()
        {
            Object.Destroy(npc.agent);
            Object.Destroy(npc.animator);
            Object.Destroy(npc);
        }
        
        private IEnumerator ScaleYAndResetPosition(Transform obj, float targetScaleY)
        {
            
            var transform = obj.transform;
            transform.gameObject.SetActive(true);
            
            var initialScale = transform.localScale;
            var targetScale = new Vector3(initialScale.x, targetScaleY, initialScale.z);

            var targetPosition = Vector3.zero; // All transform positions to (0, 0, 0)

            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var progress = elapsedTime / duration;

                obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);

                yield return null; // Wait for the next frame
            }

            obj.transform.localScale = targetScale;
            obj.transform.position = targetPosition;
        }
    }
}