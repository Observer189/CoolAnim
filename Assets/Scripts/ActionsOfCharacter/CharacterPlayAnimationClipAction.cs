using System;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public class CharacterPlayAnimationClipAction : IActionOfCharacter
    {
        [SerializeField] private AnimationClip _animationClip;

        public CharacterPlayAnimationClipAction(AnimationClip animationClip)
        {
            _animationClip = animationClip;
        }

        public void Execute(CharacterMotionController characterMotionController)
        {
            
        }

        public IActionOfCharacter Duplicate()
        {
            return new CharacterPlayAnimationClipAction(_animationClip);
        }
    }
}