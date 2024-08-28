using Npc.Ufo.Base.States.Base;
using UnityEngine;

namespace Npc.Ufo.Base.States
{
    [System.Serializable]
    public class UfoDieState : UfoState
    {
        public override void Enter()
        {
            Debug.Log("Ufo: Die");
            var items = ufo.npcFactoryManager.ufoItems;
            
            var randomItem = items[Random.Range(0, items.Count)];
            var item = Object.Instantiate(randomItem, ufo.agent.transform.position, Quaternion.identity);
            
            ufo.agent.gameObject.SetActive(false);
        }
        
        public override void Update() { }
        public override void Exit() { }
    }
}