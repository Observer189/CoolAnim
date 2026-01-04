namespace CoolAnimation
{
    public interface IActionOfCharacter
    {
        public void Execute(CharacterMotionController characterMotionController);

        public IActionOfCharacter Duplicate();
    }
}