using System;
using GraphProcessor;

namespace CoolAnimation
{
    [Serializable, NodeMenuItem("Data/TargetCharacter")]
    public class TargetCharacterMotionDataNode : MotionDataNode<CharacterMotionController>
    {
        public override BaseMotionNodeExecutable CreateExecutable()
        {
            return new TargetCharacterMotionDataNodeExecutable();
        }
    }

    public class TargetCharacterMotionDataNodeExecutable : MotionDataNodeExecutable<CharacterMotionController>
    {
        public override CharacterMotionController GetData()
        {
            return null;
        }
    }
}