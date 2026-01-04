namespace CoolAnimation
{
    public interface IConditionOfCharacter
    {
        public bool Evaluate(CharacterMotionController characterMotionController);

        public IConditionOfCharacter Duplicate();
    }
}