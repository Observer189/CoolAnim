using System;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public class CharacterFeetPlacementCondition : BaseConditionOfCharacter
    {
        [SerializeField] private FeetPlacementState _targetFeetPlacement;
        
        public CharacterFeetPlacementCondition()
        {
            
        }
        
        public CharacterFeetPlacementCondition(bool invert,FeetPlacementState targetFeetPlacement )
        {
            _invert = invert;
            _targetFeetPlacement = targetFeetPlacement;
        }

        protected override bool EvaluateInternal(CharacterMotionController characterMotionController)
        {
            return characterMotionController.FeetPlacementState == _targetFeetPlacement;
        }

        public override IConditionOfCharacter Duplicate()
        {
            return new CharacterFeetPlacementCondition(_invert, _targetFeetPlacement);
        }
    }
}