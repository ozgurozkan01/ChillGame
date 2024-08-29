using System;
using System.Collections;
using Npc.Ufo.Base.States.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Npc.Ufo.Base.States
{
    [System.Serializable]
    public class UfoIdleState : UfoState
    {
        private NavMeshAgent _navMeshAgent;
        private Vector3 _targetPosition;
        public float moveRange = 10f; // Range for random positions
        
        private float _idleTimer;
        private float _moveTimer;
        
        public float moveInterval = 2f; // Interval to pick new random positions
        public float idleDurationMin = 2f; // Minimum idle duration before transitioning to attack state
        public float idleDurationMax = 5f; // Maximum idle duration before transitioning to attack state

        public override void Init(UfoBase ufoInGame)
        {
            base.Init(ufoInGame);
            _navMeshAgent = ufoInGame.agent;
        }

        public override void Enter()
        {
            Debug.Log("Entering Idle State");
            _navMeshAgent.isStopped = false;
            _moveTimer = moveInterval;
            _idleTimer = Random.Range(idleDurationMin, idleDurationMax);
        }

        public override void Update()
        {
            // Update idle timer
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0)
            {
                ufo.SetState(ufo.ufoAttackState);
                return;
            }

            // Update move timer
            _moveTimer -= Time.deltaTime;
            if (_moveTimer <= 0)
            {
                SetRandomDestination();
                _moveTimer = moveInterval;
            }
        }

        public override void Exit()
        {
            _navMeshAgent.isStopped = true;
        }

        private void SetRandomDestination()
        {
            var randomPosition = GetRandomPosition();
            _navMeshAgent.SetDestination(randomPosition);
        }

        private Vector3 GetRandomPosition()
        {
            var randomDirection = Random.insideUnitSphere * moveRange;
            randomDirection += _navMeshAgent.transform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, moveRange, NavMesh.AllAreas);
            return hit.position;
        }
    }

}