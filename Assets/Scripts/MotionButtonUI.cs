using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoolAnimation
{
    public class MotionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        private CharacterMotion _motion;

        public event Action<CharacterMotion> OnClickMotion;
        
        public void SetMotion(CharacterMotion motion, int index)
        {
            _motion = motion;
            _text.text = $"{index}. {motion.Data.Name}";
            _button.onClick.AddListener(OnClickMotionFunc);
        }

        private void OnClickMotionFunc()
        {
            OnClickMotion?.Invoke(_motion);
        }

    }
}