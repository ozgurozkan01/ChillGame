using UnityEngine;

namespace GameManager.Base.States
{
    public class GameState
    {
        protected GameManager GameManager;

        public void Init(GameManager gameManagerInGame)
        {
            GameManager = gameManagerInGame;
        }
        
        public virtual void Enter()
        {
            Debug.Log("Game Entering");
        }

        public virtual void Update()
        {
        }
        public virtual void Exit()
        {
        }

    }
}