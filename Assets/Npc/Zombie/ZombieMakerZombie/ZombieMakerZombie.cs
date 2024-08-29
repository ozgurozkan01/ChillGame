using Npc.Zombie.Base;
using Npc.Zombie.Base.States.ZombieMakerOverrideStates;
using UnityEngine;

namespace Npc.Zombie.ZombieMakerZombie
{
    public class ZombieMakerZombie : ZombieBase
    {
        [Header("Override States")] 
        public ZombieMakerAttackState zombieMakerAttackState;

        protected override void SetAndInitStates()
        {
            attackingState = zombieMakerAttackState;
            base.SetAndInitStates();
        }


    }
}