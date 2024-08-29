using Npc.Base;
using Npc.Zombie.Base.States;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Npc.Zombie.Base
{
    public class ZombieBase : NpcBase
    {
        [Header("States")]
        public ZombieState currentState;
        public IdleState idleState;
        public ChasingState chasingState;
        public AttackingState attackingState;
        public GetDamageState getDamageState;
        public DeathState deathState;
        public ZombieWaitingState waitingState;
        public ZombieCoolDown coolDownState;

        [Header("Properties")]
        [Header("View Properties")]
        public float playerDetectionRangeCalmMode = 10f;
        public int numberOfRays = 36; // Number of rays to cast around the zombie to detect the player
        public Transform rayStartPoint;
        private Vector3[] _rayDirections;

        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
        
        [Header("Anim Key")]
        public string isWalkingAnimKey = "isWalking";
        public int isWalking;

        private void Start()
        {
            isWalking = Animator.StringToHash(isWalkingAnimKey);
            
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            
            PrecomputeRayDirections();
            
            SetAndInitStates();
            TransitionToState(waitingState); // Start in the Idle state
        }

        protected virtual void SetAndInitStates()
        {
            idleState.Init(this);
            chasingState.Init(this);
            attackingState.Init(this);
            attackingState.Init(this);
            deathState.Init(this);
            waitingState.Init(this);
            coolDownState.Init(this);
        }

        private void Update()
        {
            currentState.Update();
        }

        public void TransitionToState(ZombieState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
       
        private void PrecomputeRayDirections()
        {
            _rayDirections = new Vector3[numberOfRays];
            var angleStep = 360f / numberOfRays;

            for (int i = 0; i < numberOfRays; i++)
            {
                float angle = i * angleStep;
                _rayDirections[i] = Quaternion.Euler(0, angle, 0) * transform.forward;
            }
        }

        public bool IsPlayerClose()
        {
            foreach (var direction in _rayDirections)
            {
                Debug.DrawRay(rayStartPoint.position, direction * playerDetectionRangeCalmMode, Color.red);
                
                if (Physics.Raycast(rayStartPoint.position, direction, out var hit, playerDetectionRangeCalmMode))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true; // Player detected within range
                    }
                }
            }

            return false; // Player not detected
        }
        
        public bool IsPlayerCloseToAttack() => Vector3.Distance(transform.position, player.position) < attackingState.attackRange;
    }
}
