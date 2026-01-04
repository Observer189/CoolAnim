using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CoolAnimation
{
    public class MotionQueueVisualizerUI : MonoBehaviour
    {
        [SerializeField] private MotionButtonUI _motionButtonTemplate;
        [SerializeField] private Transform _motionButtonParent;

        [SerializeField] private MotionInQueueElemUI _motionInQueueElemTemplate;
        [SerializeField] private Transform _motionQueueParent;

        [SerializeField] private Button _executeQueueButton;

        private List<MotionButtonUI> _motionButtonList = new List<MotionButtonUI>();

        private List<MotionInQueueElemUI> _motionQueue = new List<MotionInQueueElemUI>();

        private CharacterMotionController _characterMotionController;

        public void Initialize(CharacterMotionController characterMotionController)
        {
            _characterMotionController = characterMotionController;
        }

        public void SetCharacterVisualization()
        {
            ClearCharacterVisualization();

            int ind = 0;
            foreach (var motion in _characterMotionController.GetAvailableMotions())
            {
                ind++;
                if (ind == 10)
                {
                    ind = 0;
                }

                var mButton = Instantiate(_motionButtonTemplate, _motionButtonParent);
                mButton.SetMotion(motion, ind);
                mButton.OnClickMotion += OnClickButtonMotion;
                mButton.gameObject.SetActive(true);
                
                _motionButtonList.Add(mButton);
            }
            
            _characterMotionController.OnAddMotionToQueue += OnAddMotionToQueue;
            _characterMotionController.OnRemoveMotionFromQueue += OnCharacterRemoveMotionFromQueue;
            
            _executeQueueButton.onClick.AddListener(OnClickExecuteButton);
        }

        private void OnClickExecuteButton()
        {
            _characterMotionController.ExecuteMotionQueue();
        }

        private void OnCharacterRemoveMotionFromQueue(CharacterMotion motion)
        {
            var ind = _motionQueue.FindIndex((m) => m.CharacterMotion == motion);
            if (ind >= 0)
            {
                Destroy(_motionQueue[ind].gameObject);
            }
            
            _motionQueue.RemoveAt(ind);
        }

        private void OnAddMotionToQueue(CharacterMotion motion)
        {
            var motionInQueueElem = Instantiate(_motionInQueueElemTemplate, _motionQueueParent);
            motionInQueueElem.SetMotion(motion);
            motionInQueueElem.OnClickRemoveMotion += OnClickRemoveMotion;
            motionInQueueElem.gameObject.SetActive(true);
            
            _motionQueue.Add(motionInQueueElem);
        }

        private void OnClickRemoveMotion(CharacterMotion motion)
        {
            if (_characterMotionController.MotionExecuting)
            {
                return;
            }
            
            _characterMotionController.RemoveMotionFromQueue(motion);
        }

        private void OnClickButtonMotion(CharacterMotion motion)
        {
            _characterMotionController.EnqueueMotion(motion);
        }

        public void ClearCharacterVisualization()
        {
            if (!_characterMotionController.IsNullOrDestroyed())
            {
                _characterMotionController.OnAddMotionToQueue -= OnAddMotionToQueue;
            }

            foreach (var motionButtonUI in _motionButtonList)
            {
                Destroy(motionButtonUI);
            }
            
            _motionButtonList.Clear();

            foreach (var motionInQueueElemUI in _motionQueue)
            {
                Destroy(motionInQueueElemUI);   
            }
            
            _motionQueue.Clear();
        }
    }
}