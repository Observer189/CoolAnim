using System;

namespace CoolAnimation
{
    [Serializable]
    public class SatisfyConditionsFilterOfCharacterMotion : IFilterOfCharacterMotion, IRequireOwnerCharacterData
    {
        private CharacterMotionController _owner;

        public SatisfyConditionsFilterOfCharacterMotion()
        {
            
        }
        
        public SatisfyConditionsFilterOfCharacterMotion(CharacterMotionController owner)
        {
            _owner = owner;
        }

        public bool Filter(CharacterMotion motion)
        {
            foreach (var condition in motion.MotionConditions)
            {
                if (!condition.Evaluate(_owner))
                {
                    return false;
                }
            }

            return true;
        }

        public void SetOwnerCharacterData(CharacterMotionController characterMotionController)
        {
            _owner = characterMotionController;
        }
    }
}