using System;
using Npc.Zombie.Boss.Base;
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
            var zombies = npc.npcFactoryManager.zombiesInSave;
            var randomIndex = Random.Range(0, zombies.Count);
            
            var selectedZombie = zombies[randomIndex];
            
            Object.Instantiate(selectedZombie, npc.transform.position, Quaternion.identity).gameObject.SetActive(true);
        }
        
    }
}