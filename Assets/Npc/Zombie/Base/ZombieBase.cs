using Npc.Zombie.Base.States;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Npc.Zombie.Base
{
    public class ZombieBase : MonoBehaviour
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
        public Transform player;
        public float detectionRange = 10f;

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
            
            idleState.Init(this);
            chasingState.Init(this);
            attackingState.Init(this);
            attackingState.Init(this);
            deathState.Init(this);
            waitingState.Init(this);
            coolDownState.Init(this);
            
            TransitionToState(waitingState); // Start in the Idle state
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

        public void TakeDamage(float damage)
        {
            if (currentState != deathState)
            {
                TransitionToState(getDamageState);
            }
        }
        public bool IsPlayerClose() => Vector3.Distance(transform.position, player.position) < detectionRange;
        
        public bool IsPlayerCloseToAttack() => Vector3.Distance(transform.position, player.position) < attackingState.attackRange;
    }
}
