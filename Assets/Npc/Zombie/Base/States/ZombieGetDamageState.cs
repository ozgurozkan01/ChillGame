using System;
using Npc.Zombie.Base.States.Base;
using UnityEngine;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class GetDamageState : ZombieState
    {
        public override void Enter()
        {
            Debug.Log("Entering Get Damage State");
            npc.animator.SetTrigger(npc.getDamage);
        }
        
        public void TakeDamage(float damage)
        {
            npc.health -= damage;
            
            if(npc.health <= 0)
            {
                npc.TransitionToState(npc.deathState);
            }
            else
            {
                npc.StartCoroutine(npc.WaitForAnimationToFinishCallAction(SetChase));
            }
            
        }

        public void SetChase()
        {
            npc.chasingState.SetChaseRegardlessOfRange(true);
            npc.TransitionToState(npc.chasingState);
        }
    }
}