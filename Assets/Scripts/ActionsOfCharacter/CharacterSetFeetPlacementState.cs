using System;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public class CharacterSetFeetPlacementState : IActionOfCharacter
    {
        [SerializeField] private FeetPlacementState _newFeetState;

        public CharacterSetFeetPlacementState()
        {
            
        }
        
        public CharacterSetFeetPlacementState(FeetPlacementState newFeetState)
        {
            _newFeetState = newFeetState;
        }

        public void Execute(CharacterMotionController characterMotionController)
        {
            characterMotionController.FeetPlacementState = _newFeetState;
        }

        public IActionOfCharacter Duplicate()
        {
            return new CharacterSetFeetPlacementState(_newFeetState);
        }
    }
}