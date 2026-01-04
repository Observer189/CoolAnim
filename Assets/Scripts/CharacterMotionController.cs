using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace CoolAnimation
{
    public class CharacterMotionController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private AnimationClip _defaultIdleAnimationClip;

        public Animator Animator => _animator;

        [SerializeField] private MotionControllerPreset _motionControllerPreset;

        private CharacterPlayableGraphController _playableGraphController;

        public CharacterPlayableGraphController PlayableGraphController => _playableGraphController;

        private List<CharacterMotion> _availableMotions = new List<CharacterMotion>();

        public List<CharacterMotion> AvailableMotions => _availableMotions;

        private List<CharacterMotion> _motionQueue = new List<CharacterMotion>();

        public List<CharacterMotion> MotionQueue => _motionQueue;

        private LinkedList<CharacterMotion> _motionExecutionQueue = new LinkedList<CharacterMotion>();

        private bool _motionExecuting = false;

        public bool MotionExecuting => _motionExecuting;

        private CharacterMotionGraphExecutor _motionGraphExecutor;

        private FeetPlacementState _feetPlacementState = FeetPlacementState.LeftForward;

        public FeetPlacementState FeetPlacementState
        {
            get => _feetPlacementState;
            set => _feetPlacementState = value;
        }

        public event Action<CharacterMotion> OnAddMotionToQueue;

        public event Action<CharacterMotion> OnRemoveMotionFromQueue; 

        public void Initialize()
        {
            _playableGraphController = new CharacterPlayableGraphController();
            _playableGraphController.Initialize(_animator, _defaultIdleAnimationClip);

            foreach (var motion in _motionControllerPreset.AvailableMotions)
            {
                _availableMotions.Add(motion.CreateMotion());
            }

            _motionGraphExecutor = new CharacterMotionGraphExecutor();
            _motionGraphExecutor.Initialize(this);
        }

        public void EnqueueMotion(CharacterMotion motion)
        {
            _motionQueue.Add(motion);
            OnAddMotionToQueue?.Invoke(motion);
        }

        public void RemoveMotionFromQueue(CharacterMotion motion)
        {
            _motionQueue.Remove(motion);
            OnRemoveMotionFromQueue?.Invoke(motion);
        }

        public async UniTaskVoid ExecuteMotionQueue()
        {
            if (_motionExecuting)
            {
                return;
            }

            _motionExecuting = true;
            foreach (var motion in _motionQueue)
            {
                _motionExecutionQueue.AddLast(motion);
            }

            while (_motionExecutionQueue.Count > 0)
            {
                var motion = _motionExecutionQueue.First.Value;
                _motionExecutionQueue.RemoveFirst();
                OnRemoveMotionFromQueue?.Invoke(motion);
                /*foreach (var action in motion.MotionActions)
                {
                    action.Execute(this);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(motion.EstimatedDuration));*/

                await _motionGraphExecutor.ExecuteMotionGraph(motion.Data.MotionGraph);
            }

            _motionQueue.Clear();
            _motionExecuting = false;
        }

        public List<CharacterMotion> GetAvailableMotions()
        {
            var list = new List<CharacterMotion>();

            var filter = new SatisfyConditionsFilterOfCharacterMotion(this);

            foreach (var motion in _availableMotions)
            {
                if (filter.Filter(motion))
                {
                    list.Add(motion);
                }
            }
            
            list.Sort((m1,m2)=> m1.Data.Name.CompareTo(m2.Data.Name));

            return list;
        }

        public void SubscribeToInput(GameInputController gameInputController)
        {
            gameInputController.OnPressNumber += OnPressNumber;
            gameInputController.OnSubmit += OnInputSubmit;
        }

        private void OnInputSubmit()
        {
            ExecuteMotionQueue();
        }

        public void UnsubscribeToInput(GameInputController gameInputController)
        {
            gameInputController.OnPressNumber -= OnPressNumber;
            gameInputController.OnSubmit -= OnInputSubmit;
        }

        private void OnDestroy()
        {
        }

        private void OnPressNumber(int number)
        {
            var ind = number - 1;
            if (number == 0)
            {
                ind = 9;
            }

            var availableMotions = GetAvailableMotions();

            if (availableMotions.Count <= ind)
            {
                return;
            }
            else
            {
                EnqueueMotion(availableMotions[ind]);
            }
        }

        [ContextMenu("ShowPlayableGraph")]
        public void ShowPlayableGraph()
        {
            _playableGraphController = new CharacterPlayableGraphController();
            var graph = _playableGraphController.ConstructGraph(_animator, _defaultIdleAnimationClip);
            GraphVisualizerClient.Show(graph);
        }
    }

    public enum FeetPlacementState
    {
        None,
        RightForward,
        LeftForward,
        Middle
    }
}
