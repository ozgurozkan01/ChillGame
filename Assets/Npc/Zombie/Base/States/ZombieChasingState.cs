using Npc.Zombie.Base.States.Base;
using UnityEngine;

namespace Npc.Zombie.Base.States
{
    [System.Serializable]
    public class ChasingState : ZombieState
    {
        public bool chaseRegardlessOfRange; // if u shoot to zombie, it will follow you regardless of range

        public override void Enter()
        {
            Debug.Log("Entering Chasing State");
        }

        public override void Update()
        {
            if (chaseRegardlessOfRange || npc.IsPlayerClose(playerDetectionRangeCalmMode))
            {
                npc.agent.SetDestination(npc.player.position);
                npc.animator.SetFloat(npc.isWalking, 1f); // Start walking animation
            
                if (npc.IsPlayerCloseToAttack(npc.attackingState.attackRange))
                {
                    npc.agent.SetDestination(npc.transform.position); // Stop moving
                    npc.TransitionToState(npc.attackingState); // Transition to attacking state
                }
            }
            else
            {
                npc.TransitionToState(npc.idleState);
                SetChaseRegardlessOfRange(false);
            }
        }
 

        public void SetChaseRegardlessOfRange(bool shouldChase)
        {
            chaseRegardlessOfRange = shouldChase;
        }
        
    }

}