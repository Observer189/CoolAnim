using UnityEngine;

namespace CoolAnimation
{
    [CreateAssetMenu(fileName = "MotionData", menuName = "GameData/MotionControllerPreset")]
    public class MotionControllerPreset : ScriptableObject
    {
        [SerializeField] private CharacterMotionDataSO[] _availableMotions;

        public CharacterMotionDataSO[] AvailableMotions => _availableMotions;
    }
}