using System;
using System.Collections.Generic;
using CoolAnimation;
using UnityEngine;

namespace MotionActionGraphNew
{
    [Serializable, CreateNodeMenu("Actions/ExecuteActionOnOwner"), NodeWidth(400)]
    public class ExecuteActionOfCharacterMotionNodeNew : MotionActionNodeNew
    {
        [SerializeReference, SubclassSelector] public IActionOfCharacter[] _actions;
        public override BaseMotionNodeExecutableNew CreateExecutable()
        {
            var dupActions = new List<IActionOfCharacter>();
            foreach (var action in _actions)
            {
                dupActions.Add(action.Duplicate());
            }

            return new ExecuteActionOfCharacterMotionNodeExecutableNew(dupActions);
        }
    }
    
    public class ExecuteActionOfCharacterMotionNodeExecutableNew : BaseMotionNodeExecutableNew
    {
        protected List<IActionOfCharacter> _actions;

        public ExecuteActionOfCharacterMotionNodeExecutableNew(List<IActionOfCharacter> actions)
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