using System.Collections.Generic;
using Npc.Aliens.Base;
using Npc.Zombie.Base;
using Npc.Zombie.Boss.Base;
using UnityEngine;

namespace Npc
{
    public class NpcFactoryManager : MonoBehaviour
    {
        [Header("Zombies In Save")]
        public List<ZombieBase> zombiesInSave;
        public List<BossZombieBase> bossZombiesInSave;
        
        [Header("Aliens")]
        public List<AlienBase> alienInSave;
        
        
    }
}