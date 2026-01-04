using System;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation.InstantNodes
{
    [Serializable, CreateNodeMenu("StartNode")]
    public class StartMotionNodeNew : BaseMotionNodeNew
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNodeNew _next;
        
        public override BaseMotionNodeExecutableNew CreateExecutable()
        {
            return new StartMotionNodeExecutableNew();
        }
    }

    public class StartMotionNodeExecutableNew : BaseMotionNodeExecutableNew
    {
        
    }

}