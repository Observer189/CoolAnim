using System;
using GraphProcessor;
using UnityEngine;

[Serializable, NodeMenuItem("Actions/ChangeStance")]
public class ChangeStanceMotionActionNode : MotionActionNode
{
    
    public override BaseMotionNodeExecutable CreateExecutable()
    {
        return new ChangeStanceMotionInstantActionNodeExecutable();
    }
}

public class ChangeStanceMotionInstantActionNodeExecutable : BaseMotionNodeExecutable
{
    
}
