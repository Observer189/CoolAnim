using System;

namespace CoolAnimation
{
    [Serializable, CreateNodeMenu("Actions/ChangeStance")]
    public class ChangeStanceMotionActionNodeNew : MotionActionNodeNew
    {
        public override BaseMotionNodeExecutableNew CreateExecutable()
        {
            return new ChangeStanceMotionInstantActionNodeExecutableNew();
        }
    }

    public class ChangeStanceMotionInstantActionNodeExecutableNew : BaseMotionNodeExecutableNew
    {
    
    }
}