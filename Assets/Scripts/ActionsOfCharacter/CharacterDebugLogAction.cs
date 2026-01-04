using System;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public class CharacterDebugLogAction : IActionOfCharacter
    {
        [SerializeField] private string _message;

        public CharacterDebugLogAction()
        {
            
        }
        
        public CharacterDebugLogAction(string message)
        {
            _message = message;
        }

        public void Execute(CharacterMotionController characterMotionController)
        {
            Debug.Log(_message);
        }

        public IActionOfCharacter Duplicate()
        {
            return new CharacterDebugLogAction(_message);
        }
    }
}