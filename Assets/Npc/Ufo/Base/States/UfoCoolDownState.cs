using System.Collections;
using Npc.Ufo.Base.States.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Npc.Ufo.Base.States
{
    [System.Serializable]
    public class UfoCoolDownState : UfoState
    {
        public float coolDownDuration = 2f; // Duration of cooldown in seconds

        public override void Enter()
        {
            Debug.Log("UFO is cooling down...");
            ufo.StartCoroutine(CoolDownCoroutine());
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(coolDownDuration); // Wait for the cooldown period

            ufo.SetState(ufo.ufoAttackState); // Example: return to attack state
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            Debug.Log("UFO cooldown complete.");
        }
 
    }

    

}