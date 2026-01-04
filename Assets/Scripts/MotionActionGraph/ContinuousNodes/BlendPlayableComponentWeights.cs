using System;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation.ContinuousNodes
{
    [Serializable, NodeMenuItem("ContinuousActions/BlendPlayableComponents")]
    public class BlendPlayableComponentWeights : MotionActionNode
    {
        [SerializeField] private BlendPlayableComponentData[] _targetWeights;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        
        public override BaseMotionNodeExecutable CreateExecutable()
        {
            return new BlendPlayableComponentWeightsExecutable(_targetWeights, _duration, _curve);
        }
    }

    public class BlendPlayableComponentWeightsExecutable : MotionContinuousActionNodeExecutable
    {
        private float[] _sourceWeights;
        private BlendPlayableComponentData[] _targetWeights; 
        private float _duration;
        private AnimationCurve _curve;

        public BlendPlayableComponentWeightsExecutable(BlendPlayableComponentData[] targetWeights, 
            float duration, AnimationCurve curve)
        {
            _targetWeights = targetWeights;
            _duration = duration;
            _curve = curve;
        }

        public override void Execute(CharacterMotionGraphExecutionContext context)
        {
            base.Execute(context);

            _sourceWeights = new float[_targetWeights.Length];

            for (int i = 0; i < _targetWeights.Length; i++)
            {
                _sourceWeights[i] =
                    context.MotionOwner.PlayableGraphController.GetComponentWeight(
                        _targetWeights[i].ComponentWeightType);

            }
        }

        public override void UpdateExecution(CharacterMotionGraphExecutionContext context, float deltaTime)
        {
            base.UpdateExecution(context, deltaTime);

            var t = _timeInState / _duration;

            if (t > 1)
            {
                _executed = true;
                
                for (int i = 0; i < _targetWeights.Length; i++)
                {
                    context.MotionOwner.PlayableGraphController.SetComponentWeight(_targetWeights[i].ComponentWeightType, _targetWeights[i].Weight);
                }
                
                return;
            }

            var evaluatedT = _curve.Evaluate(t);

            for (int i = 0; i < _targetWeights.Length; i++)
            {
                context.MotionOwner.PlayableGraphController.SetComponentWeight(_targetWeights[i].ComponentWeightType, 
                    _sourceWeights[i] + evaluatedT * (_targetWeights[i].Weight - _sourceWeights[i]));
            }
        }
    }

    [Serializable]
    public class BlendPlayableComponentData
    {
        public PlayableGraphComponentWeightType ComponentWeightType;
        public float Weight;
    }
}