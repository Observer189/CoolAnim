using System;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public abstract class BaseConditionOfCharacter : IConditionOfCharacter
    {
        [SerializeField] public bool _invert;
        public bool Evaluate(CharacterMotionController characterMotionController)
        {
            if (_invert)
            {
                return !EvaluateInternal(characterMotionController);
            }

            return EvaluateInternal(characterMotionController);
        }

        protected abstract bool EvaluateInternal(CharacterMotionController characterMotionController);

        public abstract IConditionOfCharacter Duplicate();
    }
}