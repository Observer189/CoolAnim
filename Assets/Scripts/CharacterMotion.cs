using System.Collections.Generic;

namespace CoolAnimation
{
    public class CharacterMotion
    {
        private CharacterMotionDataSO _data;

        public CharacterMotionDataSO Data => _data;

        private List<IConditionOfCharacter> _motionConditions;

        public List<IConditionOfCharacter> MotionConditions => _motionConditions;

        private List<IActionOfCharacter> _motionActions;

        public List<IActionOfCharacter> MotionActions => _motionActions;

        private float _estimatedDuration;

        public float EstimatedDuration => _estimatedDuration;

        public CharacterMotion(CharacterMotionDataSO data, List<IConditionOfCharacter> motionConditions, 
            List<IActionOfCharacter> motionActions, float estimatedDuration)
        {
            _data = data;
            _motionConditions = motionConditions;
            _motionActions = motionActions;
            _estimatedDuration = estimatedDuration;
        }
    }
}