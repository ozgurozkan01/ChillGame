using System;
using System.Collections;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class IdleState : ZombieState
    {
        private Vector3 _targetPosition;
        public float walkRadiusRange = 25f; // Radius within which to move randomly
        public float playerDetectionRange;
        
        public override void Enter()
        {
            Debug.Log("Idle State");

            SetRandomTargetPosition();
            
            npc.animator.SetFloat(npc.isWalking, 1f); // Start walking animation
        }

        public override void Update()
        {
            npc.agent.SetDestination(_targetPosition);

            if (Vector3.Distance(npc.transform.position, _targetPosition) < 1f)
            {
                npc.TransitionToState(npc.waitingState); // Transition to waiting state
            }

            if (npc.IsPlayerClose(playerDetectionRange))
            {
                npc.TransitionToState(npc.chasingState);
            }
        }

        private void SetRandomTargetPosition()
        {
            var randomDirection = npc.transform.position + Random.insideUnitSphere * walkRadiusRange;
            if (NavMesh.SamplePosition(randomDirection, out var hit, walkRadiusRange, NavMesh.AllAreas))
            {
                _targetPosition = hit.position;
            }
        }
        

        
    }
}