using System;
using UnityEngine.Serialization;

namespace Npc.Zombie.Base.States.Base
{
    [Serializable]
    public class ZombieState
    {
        public ZombieBase npc;

        public virtual void Init(ZombieBase npcInGame)
        {
            npc = npcInGame;
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void Exit()
        {
            
        }
        
    }
}