using System;
using System.Collections.Generic;
using CoolAnimation;
using UnityEngine;
using XNode;

namespace MotionActionGraphNew.InstantNodes
{
    [Serializable, CreateNodeMenu("Conditions/CharacterCondition"), NodeWidth(400)]
    public class CharacterConditionMotionNode : BaseMotionNode
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        public BaseMotionNode _trueNext;
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        public BaseMotionNode _falseNext;
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        public BaseMotionNode _anyNext;
        
        [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited), SerializeField]
        protected BaseMotionNode _prev;
        
        [SerializeReference, SubclassSelector] public IConditionOfCharacter _condition;
        public override BaseMotionNodeExecutable CreateExecutable()
        {
            return new CharacterConditionMotionNodeExecutable(_condition);
            
        }
    }

    public class CharacterConditionMotionNodeExecutable : BaseMotionNodeExecutable
    {
        protected IConditionOfCharacter _condition;

        protected List<BaseMotionNodeExecutable> _nextTrueNodes = new List<BaseMotionNodeExecutable>();

        protected List<BaseMotionNodeExecutable> _nextFalseNodes = new List<BaseMotionNodeExecutable>();

        protected List<BaseMotionNodeExecutable> _nextAnyNodes = new List<BaseMotionNodeExecutable>();

        private bool _evaluationResult = false;
        public CharacterConditionMotionNodeExecutable(IConditionOfCharacter condition)
        {
            _condition = condition;
        }

        public override void CreateConnections(BaseMotionNode ownTemplate, Dictionary<BaseMotionNode, BaseMotionNodeExecutable> templateAssociations)
        {
            base.CreateConnections(ownTemplate, templateAssociations);


            var truePort = ownTemplate.GetPort("_trueNext");
            var falsePort = ownTemplate.GetPort("_falseNext");
            var anyPort = ownTemplate.GetPort("_anyNext");

            FillNextNodesByPort(_nextTrueNodes, truePort, templateAssociations);
            FillNextNodesByPort(_nextFalseNodes, falsePort, templateAssociations);
            FillNextNodesByPort(_nextAnyNodes, anyPort, templateAssociations);
        }

        private void FillNextNodesByPort(List<BaseMotionNodeExecutable> listToFill, NodePort port,
            Dictionary<BaseMotionNode, BaseMotionNodeExecutable> templateAssociations)
        {
            foreach (var nodePort in port.GetConnections())
            {
                if (nodePort.node is BaseMotionNode motionNode)
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

        public override List<BaseMotionNodeExecutable> GetNextNodes()
        {
            var nextNodes = new List<BaseMotionNodeExecutable>(_nextAnyNodes);

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