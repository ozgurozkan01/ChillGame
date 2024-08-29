using System;
using Npc.Zombie.Base.States.Base;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Npc.Zombie.Base.States
{
    [Serializable]
    public class AttackingState : ZombieState
    {
        public float attackRange = 2f;

        public override void Enter()
        {
            Debug.Log("Attack State");
            npc.animator.SetTrigger(npc.attack); // Start attacking animation

            npc.StartCoroutine(npc.
                WaitForAnimationToFinishCallAction(
                    () =>
                    {
                        npc.TransitionToState(npc.coolDownState);
                        
                    }
                )
            );
            // send ray
        }

    }
}