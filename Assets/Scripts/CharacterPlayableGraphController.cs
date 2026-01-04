using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace CoolAnimation
{
    public class CharacterPlayableGraphController
    {
        private PlayableGraph _playableGraph;

        public PlayableGraph PlayableGraph => _playableGraph;

        private Animator _animator;

        private AnimationClip _defaultIdleAnimationClip;

        private AnimationClipPlayable _idleAnimationPlayable;

        private AnimationClipPlayable _motionAnimationPlayable;

        private AnimationMixerPlayable _finalMixerPlayable;

        private Dictionary<AnimationClip, AnimationClipPlayable> _animationPlayableCache = new Dictionary<AnimationClip, AnimationClipPlayable>();
        public void Initialize(Animator animator, AnimationClip defaultIdleAnimationClip)
        {
            _animator = animator;
            _defaultIdleAnimationClip = defaultIdleAnimationClip;

            _playableGraph = ConstructGraph(animator, defaultIdleAnimationClip);
            
            _playableGraph.Play();
        }

        public PlayableGraph ConstructGraph(Animator animator, AnimationClip defaultIdleAnimationClip)
        {
            _playableGraph = PlayableGraph.Create();
            _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            
            var playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", animator);

            _finalMixerPlayable = AnimationMixerPlayable.Create(_playableGraph, 2);
            //_idleAnimationPlayable = AnimationClipPlayable.Create(_playableGraph, defaultIdleAnimationClip);
            _idleAnimationPlayable = GetOrCreateAnimationPlayable(_defaultIdleAnimationClip);
            _motionAnimationPlayable = AnimationClipPlayable.Create(_playableGraph, defaultIdleAnimationClip);

            _playableGraph.Connect(_idleAnimationPlayable, 0, _finalMixerPlayable, 0);
            _playableGraph.Connect(_motionAnimationPlayable, 0, _finalMixerPlayable, 1);
            
            _finalMixerPlayable.SetInputWeight(0, 1f);
            _finalMixerPlayable.SetInputWeight(1, 0f);
            
            playableOutput.SetSourcePlayable(_finalMixerPlayable);

            return _playableGraph;
        }

        public void ChangeIdleAnimation(AnimationClip animationClip)
        {
            var inputWeight = _finalMixerPlayable.GetInputWeight(0);
            // Disconnect mixer input.
            _finalMixerPlayable.DisconnectInput(0);

            // Create new clip playable
            _idleAnimationPlayable = GetOrCreateAnimationPlayable(animationClip);

            // Plug it back into the mixer
            _playableGraph.Connect(_idleAnimationPlayable, 0, _finalMixerPlayable, 0);
            
            _finalMixerPlayable.SetInputWeight(0, inputWeight);
        }

        public void ChangeMotionAnimation(AnimationClip animationClip)
        {
            var inputWeight = _finalMixerPlayable.GetInputWeight(1);
            
            _finalMixerPlayable.DisconnectInput(1);

            _motionAnimationPlayable = GetOrCreateAnimationPlayable(animationClip);

            _playableGraph.Connect(_motionAnimationPlayable, 0, _finalMixerPlayable, 1);
            
            _finalMixerPlayable.SetInputWeight(1, inputWeight);
        }

        public void SetComponentWeight(PlayableGraphComponentWeightType componentWeightType, float weight)
        {
            switch (componentWeightType)
            {
                case PlayableGraphComponentWeightType.IdleAnim:
                    _finalMixerPlayable.SetInputWeight(0, weight);
                    break;
                case PlayableGraphComponentWeightType.MotionAnim:
                    _finalMixerPlayable.SetInputWeight(1, weight);
                    break;
            }
        }
        
        public float GetComponentWeight(PlayableGraphComponentWeightType componentWeightType)
        {
            switch (componentWeightType)
            {
                case PlayableGraphComponentWeightType.IdleAnim:
                    return _finalMixerPlayable.GetInputWeight(0);
                case PlayableGraphComponentWeightType.MotionAnim:
                    return _finalMixerPlayable.GetInputWeight(1);
            }

            return float.MaxValue;
        }

        private AnimationClipPlayable GetOrCreateAnimationPlayable(AnimationClip animationClip)
        {
            //Если всё таки захочется использовать кеширование, то надо не забыть обнулять время 
            //при новом использовании
            return AnimationClipPlayable.Create(_playableGraph, animationClip);
            
            /*if (_animationPlayableCache.TryGetValue(animationClip, out var playable))
            {
                return playable;
            }
            else
            {
                var p = AnimationClipPlayable.Create(_playableGraph, animationClip);
                _animationPlayableCache.Add(animationClip, p);

                return p;
            }*/
        }
    }

    public enum PlayableGraphComponentWeightType
    {
        IdleAnim, MotionAnim
    }
}