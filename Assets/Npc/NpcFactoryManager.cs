using System.Collections.Generic;
using Npc.Aliens;
using Npc.Aliens.Base;
using Npc.Base;
using Npc.Ufo.DropItem;
using Npc.Zombie.Base;
using Npc.Zombie.Boss.Base;
using SoundManager.Base;
using UnityEngine;

namespace Npc
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