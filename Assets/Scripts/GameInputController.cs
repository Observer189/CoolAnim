using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoolAnimation
{
    public class GameInputController : MonoBehaviour
    {
        private PlayerActionMaps _playerActionMaps;

        public event Action<int> OnPressNumber;

        public event Action OnSubmit; 
        

        public void Initialize()
        {
            _playerActionMaps = new PlayerActionMaps();
        }

        public void Subscribe()
        {
            _playerActionMaps.Player.PressNumber.performed += OnPressNumberPerformed;
            _playerActionMaps.UI.Submit.performed += OnSubmitPerformed;
        }

        public void Unsubscribe()
        {
            _playerActionMaps.Player.PressNumber.performed -= OnPressNumberPerformed;
            _playerActionMaps.UI.Submit.performed -= OnSubmitPerformed;
        }

        public void ActivateInput()
        {
            _playerActionMaps.Enable();
        }

        public void DeactivateInput()
        {
            _playerActionMaps.Disable();
        }

        public void Dispose()
        {
            _playerActionMaps.Dispose();
        }

        private void OnSubmitPerformed(InputAction.CallbackContext ctx)
        {
            OnSubmit?.Invoke();
        }

        private void OnDestroy()
        {
          
        }

        private void OnPressNumberPerformed(InputAction.CallbackContext ctx)
        {
            OnPressNumber?.Invoke(Mathf.RoundToInt(ctx.ReadValue<float>()));
        }
    }
}