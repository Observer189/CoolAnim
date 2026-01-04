using System;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable, NodeMenuItem("StartNode")]
    public class StartMotionNode : BaseMotionNode
    {
        [Output(name = "Next", allowMultiple = true), SerializeField]
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