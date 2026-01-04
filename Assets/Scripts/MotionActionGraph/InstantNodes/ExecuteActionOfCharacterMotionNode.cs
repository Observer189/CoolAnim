using System;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation
{
    [Serializable, NodeMenuItem("Actions/ExecuteActionOnOwner")]
    public class ExecuteActionOfCharacterMotionNode : MotionActionNode
    {
        [SerializeReference, SubclassSelector] public IActionOfCharacter[] _actions;
        public override BaseMotionNodeExecutable CreateExecutable()
        {
            var dupActions = new List<IActionOfCharacter>();
            foreach (var action in _actions)
            {
                dupActions.Add(action.Duplicate());
            }

            return new ExecuteActionOfCharacterMotionNodeExecutable(dupActions);
        }
    }

    public class ExecuteActionOfCharacterMotionNodeExecutable : BaseMotionNodeExecutable
    {
        protected List<IActionOfCharacter> _actions;

        public ExecuteActionOfCharacterMotionNodeExecutable(List<IActionOfCharacter> actions)
        {
            _actions = actions;
        }

        public override void Execute(CharacterMotionGraphExecutionContext context)
        {
            foreach (var action in _actions)
            {
                action.Execute(context.MotionOwner);
            }
            base.Execute(context);
        }
    }
}