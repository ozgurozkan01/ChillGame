using Npc.Base;
using UnityEngine;

namespace Npc.Ufo.DropItem
{
    public class Beggar : NpcBase
    {
        public int pkkMusicId;
        public int turkishMusicId;
        
        public void GotHelpFromPlayer()
        {
            Debug.Log("Beggar: Got help from player");
            npcFactoryManager.gameManager.soundManager.PlaySound(pkkMusicId);
        }
        public void GotKilledByPlayer()
        {
            Debug.Log("Beggar: Got killed by player");
            npcFactoryManager.gameManager.soundManager.PlaySound(turkishMusicId);
        }
        
        
    }
}