using System;
using System.Collections;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class IdleState : ZombieState
    {
        private Vector3 _targetPosition;

        public float walkRadius = 25f; // Radius within which to move randomly

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

            if (npc.IsPlayerClose())
            {
                npc.TransitionToState(npc.chasingState);
            }
        }

        private void SetRandomTargetPosition()
        {
            var randomDirection = npc.transform.position + Random.insideUnitSphere * walkRadius;
            if (NavMesh.SamplePosition(randomDirection, out var hit, walkRadius, NavMesh.AllAreas))
            {
                _targetPosition = hit.position;
            }
        }
        

        
    }
}