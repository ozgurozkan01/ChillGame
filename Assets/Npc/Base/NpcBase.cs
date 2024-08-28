using UnityEngine;

namespace Npc.Base
{
    public class NpcBase : MonoBehaviour
    {
        public Transform player;
        [Header("Managers")]
        public NpcFactoryManager npcFactoryManager;

    }
}
