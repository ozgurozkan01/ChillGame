using Npc.Base;
using Npc.Ufo.Base.States;
using Npc.Ufo.Base.States.Base;
using UnityEngine;
using UnityEngine.AI;

namespace Npc.Ufo.Base
{
    public class UfoBase : NpcBase
    {
        private UfoState _currentState;

        public UfoIdleState ufoIdleState;
        public UfoCoolDownState ufoCoolDownState;
        public UfoGetDamageState ufoGetDamageState;
        public UfoDieState ufoDieState;
        public UfoAttackState ufoAttackState;
        
        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
        
        public void Start()
        {
            ufoIdleState.Init(this);
            ufoCoolDownState.Init(this);
            ufoGetDamageState.Init(this);
            ufoDieState.Init(this);
            ufoAttackState.Init(this);

            SetState(ufoIdleState);
        }
        
        public void SetState(UfoState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }

        
    }
}