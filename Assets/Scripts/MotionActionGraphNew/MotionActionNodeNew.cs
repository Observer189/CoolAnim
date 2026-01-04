using System;
using CoolAnimation;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable]
    public abstract class MotionActionNodeNew : BaseMotionNodeNew
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNodeNew _next;
        [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNodeNew _previous;
    }
}

public abstract class MotionContinuousActionNodeExecutableNew : BaseMotionNodeExecutableNew
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