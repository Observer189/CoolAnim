using System;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation.InstantNodes
{
    [Serializable, CreateNodeMenu("StartNode")]
    public class StartMotionNode : BaseMotionNode
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNode _next;
        
        public override BaseMotionNodeExecutable CreateExecutable()
        {
            return new StartMotionNodeExecutable();
        }
    }

    public class StartMotionNodeExecutable : BaseMotionNodeExecutable
    {
        
    }

}