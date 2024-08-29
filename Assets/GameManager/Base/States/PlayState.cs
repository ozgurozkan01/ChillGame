using System;
using UnityEngine;

namespace GameManager.Base.States
{
    [Serializable]
    public class PlayState : GameState
    {
        public override void Enter()
        {
            Debug.Log("Game Entering");
        }

        public override void Update()
        {
        }
        public override void Exit()
        {
        }
    }
}