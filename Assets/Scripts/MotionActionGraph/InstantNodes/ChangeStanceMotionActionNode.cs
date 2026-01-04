using System;

namespace CoolAnimation
{
    [Serializable, CreateNodeMenu("Actions/ChangeStance")]
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
}