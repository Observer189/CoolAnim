using System;
using UnityEngine;
using XNode;

namespace CoolAnimation
{
    [Serializable, CreateNodeMenu("ContinuousActions/PlayMotion")]
    public class PlayMotionContinuousNode : MotionActionNode
    {
        [SerializeField] private AnimationClip _motionClip;
        [SerializeField] private float _duration;
        [SerializeField] private bool _setDurationBasedOnClip;
        public override BaseMotionNodeExecutable CreateExecutable()
        {
            return new PlayMotionContinuousNodeExecutable(_motionClip, _duration, _setDurationBasedOnClip);
        }
    }
    
    public class PlayMotionContinuousNodeExecutable : MotionContinuousActionNodeExecutable
    {
        private AnimationClip _motionClip;
        private float _duration;
        private bool _setDurationBasedOnClip;

        public PlayMotionContinuousNodeExecutable(AnimationClip clip, float duration, bool setDurationBasedOnClip)
        {
            _motionClip = clip;
            _duration = duration;
            _setDurationBasedOnClip = setDurationBasedOnClip;
        }

        public override void Execute(CharacterMotionGraphExecutionContext context)
        {
            base.Execute(context);
            context.MotionOwner.PlayableGraphController.ChangeMotionAnimation(_motionClip);
        }

        public override void UpdateExecution(CharacterMotionGraphExecutionContext context, float deltaTime)
        {
            base.UpdateExecution(context, deltaTime);

            var finalDuration = (_setDurationBasedOnClip) ? _motionClip.length : _duration;
            //Debug.Log($"Final dur = {finalDuration}, time in state = {_timeInState}");

            if (_timeInState > finalDuration)
            {
                _executed = true;
            }
        }
    }
}