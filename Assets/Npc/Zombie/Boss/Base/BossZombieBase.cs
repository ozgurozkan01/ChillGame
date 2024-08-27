using Npc.Zombie.Base;
using Npc.Zombie.Base.States.BossOverrideState;
using UnityEngine;

namespace Npc.Zombie.Boss.Base
{
    public class BossZombieBase : ZombieBase
    {
        [Header("Override States")]
        public BossAttackState bossAttackingState;

        public Weapon.Base.Weapon weapon;

        public override void SetAndInitStates()
        {
            attackingState = bossAttackingState;
            weapon.playerPoint = player;
            
            base.SetAndInitStates();
        }
        public void Shoot()
        {
            weapon.Fire();
        }
    }
}