using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation
{
    [CreateAssetMenu(fileName = "MotionData", menuName = "GameData/CharacterMotion")]
    public class CharacterMotionDataSO : ScriptableObject, IAssetWithId
    {

        [SerializeField] private string _name;

        public string Name => _name;

        [SerializeField] private float _estimatedDuration;

        public float EstimatedDuration => _estimatedDuration;

        [SerializeReference, SubclassSelector] private IConditionOfCharacter[] _conditions;

        [SerializeReference, SubclassSelector] private IActionOfCharacter[] _actions;

        [SerializeField] private CharacterMotionGraphNew _motionGraph;

        public CharacterMotionGraphNew MotionGraph => _motionGraph;

        public CharacterMotion CreateMotion()
        {
            var dupConditions = new List<IConditionOfCharacter>(_conditions.Length);

            foreach (var cond in _conditions)
            {
                dupConditions.Add(cond);
            }
            
            var dupActions = new List<IActionOfCharacter>(_actions.Length);

            foreach (var action in _actions)
            {
                dupActions.Add(action);
            }

            return new CharacterMotion(this, dupConditions, dupActions, _estimatedDuration);
        }
    }
}