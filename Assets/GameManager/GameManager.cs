using System;
using System.Collections.Generic;
using GameManager.Base.States;
using GameManager.FactoryManager;
using Npc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum Modes
{
    MainMenu,
    Play,
}

[Serializable]
public class UIState
{
    public Modes mode = Modes.MainMenu;
    public List<Transform> uiElements = new List<Transform>();
    public List<Button> startModeButtons = new List<Button>();
}

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        public SoundManager.SoundManager soundManager;
        public NpcFactoryManager factoryManager;
        public SaveManager.SaveManager saveManager;
        
        public Transform player;

        private GameState _currentState;
        public MainMenuState mainMenuState;
        public PlayState playState;
        
        public List<UIState> uiStates = new List<UIState>();
        
        public void Start()
        {
            SetUIElements();
            
            mainMenuState.Init(this);
            playState.Init(this);

            SetState(mainMenuState);
        }
        
        public void SetState(GameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Update();
        }

        private void SetUIElements()
        {
            foreach (var uiState in uiStates) // states
            {
                foreach (var uiButton in uiState.startModeButtons) // state buttons
                {
                    uiButton.onClick.AddListener(() =>
                    {
                        foreach (var uiElement in uiStates) // other transform are false
                        {
                            SetStateUIElement(uiElement.uiElements, false);       
                        }
                        
                        SetStateUIElement(uiState.uiElements, true); // current transform are true        
                        
                        SetState(uiState.mode switch // temp solution
                        {
                            Modes.MainMenu => mainMenuState,
                            Modes.Play => playState,
                        });
                        
                    });
                }
            }
        }

        private void SetStateUIElement(List<Transform> uiStateUIElements, bool state)
        {
            foreach (var elementUIElement in uiStateUIElements)
            {
                elementUIElement.gameObject.SetActive(state);
            }
        }
    }
}