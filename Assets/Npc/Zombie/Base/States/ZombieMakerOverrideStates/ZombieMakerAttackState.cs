using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Npc.Zombie.Base.States.ZombieMakerOverrideStates
{
    [Serializable]
    public class ZombieMakerAttackState : AttackingState
    {
        public override void Enter()
        {
            Debug.Log("Attack State Boss");
            SpawnRandomZombie();

            npc.TransitionToState(npc.coolDownState);
        }
        private void SpawnRandomZombie()
        {
           npc.npcFactoryManager.SpawnRandomZombieBase(npc.transform.position);
        }
        
    }
}