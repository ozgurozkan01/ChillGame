using System;
using System.Collections;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class ZombieWaitingState : ZombieState
    {
        public float minWaitTime = 2f; // Minimum wait time in seconds
        public float maxWaitTime = 5f; // Maximum wait time in seconds
        public float waitTime;
        public float playerDetectionRange = 10f; // Range within which to detect player
        
        public override void Enter()
        {
            Debug.Log("Waiting State");

            npc.animator.SetFloat(npc.isWalking, 0f); // Reset walking animation
            npc.StartCoroutine(WaitCoroutine());
        }
        public override void Update()
        {
            if (npc.IsPlayerClose(playerDetectionRange))
            {
                npc.StopAllCoroutines(); // Stop waiting if player is detected
                npc.TransitionToState(npc.chasingState); // Transition to Chasing state
            }
        }
        private IEnumerator WaitCoroutine()
        {
            waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            npc.TransitionToState(npc.idleState);
        }
    }
}