namespace CoolAnimation
{
    public class CharacterMotionGraphExecutionContext
    {
        private CharacterMotionController _motionOwner;

        public CharacterMotionController MotionOwner => _motionOwner;

        public void Initialize(CharacterMotionController motionOwner)
        {
            _motionOwner = motionOwner;
        }
    }
}