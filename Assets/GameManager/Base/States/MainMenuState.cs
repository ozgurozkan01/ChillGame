using System;
using UnityEngine;

namespace GameManager.Base.States
{
    [Serializable]
    public class MainMenuState : GameState
    {
        public override void Enter()
        {
            Debug.Log("Main Menu Entering");
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
        }
    }
}