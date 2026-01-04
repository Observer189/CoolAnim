using System;
using UnityEngine;

namespace CoolAnimation
{
    public class GameContainer : MonoBehaviour
    {
        [SerializeField] private UIContainer _uiContainer;

        [SerializeField] private GameInputController _inputController;

        [SerializeField] private CharacterMotionController _characterMotionController;

        private void Start()
        {
            InitializeAll();
            SubscribeAll();
            
            _inputController.ActivateInput();
        }

        private void InitializeAll()
        {
            _inputController.Initialize();
            _characterMotionController.Initialize();
            _uiContainer.Initialize(_characterMotionController);
        }

        private void SubscribeAll()
        {
            _inputController.Subscribe();
            _characterMotionController.SubscribeToInput(_inputController);
            _uiContainer.Subscribe();
        }
    }
}