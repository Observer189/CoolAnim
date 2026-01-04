using UnityEngine;

namespace CoolAnimation
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private MotionQueueVisualizerUI _motionQueueVisualizer;
        
        private CharacterMotionController _characterMotionController;
        public void Initialize(CharacterMotionController characterMotionController)
        {
            _characterMotionController = characterMotionController;
            _motionQueueVisualizer.Initialize(_characterMotionController);
        }

        public void Subscribe()
        {
            _motionQueueVisualizer.SetCharacterVisualization();
        }
    }
}