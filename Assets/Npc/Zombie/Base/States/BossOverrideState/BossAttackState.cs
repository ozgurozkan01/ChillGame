using System;
using Npc.Zombie.Boss.Base;
using UnityEngine;

namespace Npc.Zombie.Base.States.BossOverrideState
{
    [Serializable]
    public class BossAttackState : AttackingState
    {
        public override void Enter()
        {
            Debug.Log("Attack State Boss");
            BossZombieBase bossZombieBase = (BossZombieBase)npc;
            bossZombieBase.Shoot();
            npc.TransitionToState(npc.coolDownState);
            
        }
    }
}