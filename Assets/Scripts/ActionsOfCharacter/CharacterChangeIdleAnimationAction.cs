using System;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public class CharacterChangeIdleAnimationAction : IActionOfCharacter
    {
        [SerializeField] private AnimationClip _animationClip;

        public CharacterChangeIdleAnimationAction()
        {
            
        }
        
        public CharacterChangeIdleAnimationAction(AnimationClip animationClip)
        {
            _animationClip = animationClip;
        }
        public void Execute(CharacterMotionController characterMotionController)
        {
            characterMotionController.PlayableGraphController.ChangeIdleAnimation(_animationClip);
        }

        public IActionOfCharacter Duplicate()
        {
            return new CharacterChangeIdleAnimationAction(_animationClip);
        }
    }
}