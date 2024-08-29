using System.Collections.Generic;
using GameManager.Base;
using Npc.Aliens.Base;
using Npc.Base;
using Npc.Zombie.Base;
using Npc.Zombie.Boss.Base;
using UnityEngine;

namespace GameManager.FactoryManager
{
    public class NpcFactoryManager : ManagerBase
    {
        [Header("Zombies In Save")]
        public List<ZombieBase> zombiesInSave;
        public List<BossZombieBase> bossZombiesInSave;
        
        [Header("Aliens")]
        public List<AlienBase> alienInSave;

        [Header("Ufo Items In Save")] 
        public List<NpcBase> ufoItems;
        
        
        

    }
}