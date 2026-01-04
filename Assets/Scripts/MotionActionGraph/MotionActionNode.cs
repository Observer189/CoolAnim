using System;
using CoolAnimation;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public abstract class MotionActionNode : BaseMotionNode
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNode _next;
        [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNode _previous;
        
    }
}

public abstract class MotionContinuousActionNodeExecutable : BaseMotionNodeExecutable
{

    protected float _timeInState;

    public override void Execute(CharacterMotionGraphExecutionContext context)
    {
       
    }

    public virtual void UpdateExecution(CharacterMotionGraphExecutionContext context, float deltaTime)
    {
        _timeInState += deltaTime;
    }

    public override void ResetNodeState()
    {
        base.ResetNodeState();
        _timeInState = 0;
    }
}