using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoolAnimation
{
    public class MotionInQueueElemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _removeButton;

        private CharacterMotion _characterMotion;

        public CharacterMotion CharacterMotion => _characterMotion;

        public event Action<CharacterMotion> OnClickRemoveMotion; 

        public void SetMotion(CharacterMotion characterMotion)
        {
            _characterMotion = characterMotion;
            _text.text = characterMotion.Data.Name;
            
            _removeButton.onClick.AddListener(OnClickRemoveMotionButton);
        }

        private void OnClickRemoveMotionButton()
        {
            OnClickRemoveMotion?.Invoke(_characterMotion);
        }
    }
}