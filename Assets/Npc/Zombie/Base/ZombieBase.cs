using System;
using System.Collections;
using Npc.Base;
using Npc.Zombie.Base.States;
using Npc.Zombie.Base.States.Base;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
        public int numberOfRays = 36; // Number of rays to cast around the zombie to detect the player
        public Transform rayStartPoint;
        private Vector3[] _rayDirections;
        [Header("Health Properties")]
        public float health = 100f;
        public Transform holyCube;
        
        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
        
        [Header("Anim Key")]
        public string isWalkingAnimKey = "isWalking";
        public int isWalking;
        
        public string attackKey = "Attack";
        public int attack;

        public string getDamageKey = "GetDamage";
        public int getDamage;

        public int death; 
        public string deathKey = "Death";

        public void Init()
        {
            isWalking = Animator.StringToHash(isWalkingAnimKey);
            attack = Animator.StringToHash(attackKey);
            getDamage = Animator.StringToHash(getDamageKey);
            death = Animator.StringToHash(deathKey);
            
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            
            PrecomputeRayDirections();  
            
            SetAndInitStates();
        }

        protected virtual void SetAndInitStates()
        {
            idleState.Init(this);
            chasingState.Init(this);
            attackingState.Init(this);
            getDamageState.Init(this);
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

        public bool IsPlayerClose(float range, Color color = default)
        {
            if (color == default) // hate for this method
            {
                color = Color.red; // Set default color to red
            }
            
            foreach (var direction in _rayDirections)
            {
                Debug.DrawRay(rayStartPoint.position, direction * range, color);
                
                if (Physics.Raycast(rayStartPoint.position, direction, out var hit, range))
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
        
        public bool IsPlayerCloseToAttack(float range)
        {
            var position = rayStartPoint.position;
            
            var directionToPlayer = (player.position - position).normalized;

            Debug.DrawRay(position, directionToPlayer * range, Color.blue);
    
            if (Physics.Raycast(rayStartPoint.position, directionToPlayer, out var hit, range))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Player"))
                {
                    return true; // Player detected within range
                }
            }

            return false; // Player not detected
        }
        
        public IEnumerator WaitForAnimationToFinishCallAction(Action afterAnimationFinished)
        {
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            afterAnimationFinished?.Invoke();
        }

        private void OnTriggerEnter(Collider other) // temporary solution
        {
            if (other.CompareTag("Player"))
            {
                TakeDamage(10);
            }
        }

        // call when it gets damage
        public void TakeDamage(float damage)
        {
            TransitionToState(getDamageState);
            getDamageState.TakeDamage(damage);
        }
    }
}
