using System;
using System.Collections.Generic;
using CoolAnimation;
using UnityEngine;
using XNode;

namespace MotionActionGraphNew.InstantNodes
{
    [Serializable, CreateNodeMenu("Conditions/CharacterCondition"), NodeWidth(400)]
    public class CharacterConditionMotionNodeNew : BaseMotionNodeNew
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        public BaseMotionNodeNew _trueNext;
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        public BaseMotionNodeNew _falseNext;
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        public BaseMotionNodeNew _anyNext;
        
        [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNodeNew _prev;
        
        [SerializeReference, SubclassSelector] public IConditionOfCharacter _condition;
        public override BaseMotionNodeExecutableNew CreateExecutable()
        {
            return new CharacterConditionMotionNodeExecutableNew(_condition);
            
        }
    }

    public class CharacterConditionMotionNodeExecutableNew : BaseMotionNodeExecutableNew
    {
        protected IConditionOfCharacter _condition;

        protected List<BaseMotionNodeExecutableNew> _nextTrueNodes = new List<BaseMotionNodeExecutableNew>();

        protected List<BaseMotionNodeExecutableNew> _nextFalseNodes = new List<BaseMotionNodeExecutableNew>();

        protected List<BaseMotionNodeExecutableNew> _nextAnyNodes = new List<BaseMotionNodeExecutableNew>();

        private bool _evaluationResult = false;
        public CharacterConditionMotionNodeExecutableNew(IConditionOfCharacter condition)
        {
            _condition = condition;
        }

        public override void CreateConnections(BaseMotionNodeNew ownTemplate, Dictionary<BaseMotionNodeNew, BaseMotionNodeExecutableNew> templateAssociations)
        {
            base.CreateConnections(ownTemplate, templateAssociations);


            var truePort = ownTemplate.GetPort("_trueNext");
            var falsePort = ownTemplate.GetPort("_falseNext");
            var anyPort = ownTemplate.GetPort("_anyNext");

            FillNextNodesByPort(_nextTrueNodes, truePort, templateAssociations);
            FillNextNodesByPort(_nextFalseNodes, falsePort, templateAssociations);
            FillNextNodesByPort(_nextAnyNodes, anyPort, templateAssociations);
        }

        private void FillNextNodesByPort(List<BaseMotionNodeExecutableNew> listToFill, NodePort port,
            Dictionary<BaseMotionNodeNew, BaseMotionNodeExecutableNew> templateAssociations)
        {
            foreach (var nodePort in port.GetConnections())
            {
                if (nodePort.node is BaseMotionNodeNew motionNode)
                {
                    if (templateAssociations.TryGetValue(motionNode, out var motionNodeExecutable))
                    {
                        listToFill.Add(motionNodeExecutable);
                    }
                    else
                    {
                        Debug.LogError($"There is no node with name {motionNode.name} in templateAssociations");
                    }
                }
                else
                {
                    Debug.LogError($"Serialized edge has wrong output node type = {nodePort.node.GetType()}");
                }
            }
        }

        public override void Execute(CharacterMotionGraphExecutionContext context)
        {
            base.Execute(context);
            _evaluationResult = _condition.Evaluate(context.MotionOwner);
        }

        public override List<BaseMotionNodeExecutableNew> GetNextNodes()
        {
            var nextNodes = new List<BaseMotionNodeExecutableNew>(_nextAnyNodes);

            if (_evaluationResult)
            {
                nextNodes.AddRange(_nextTrueNodes);
            }
            else
            {
                nextNodes.AddRange(_nextFalseNodes);
            }

            return nextNodes;
        }
    }
}