using System.Collections.Generic;
using UnityEngine;

namespace CoolAnimation
{
    public class CharacterMotionGraphExecutable
    {
        private List<BaseMotionNodeExecutable> _allNodes = new List<BaseMotionNodeExecutable>();

        public List<BaseMotionNodeExecutable> AllNodes => _allNodes;

        private HashSet<MotionContinuousActionNodeExecutable> _activeContinuousNodes =
            new HashSet<MotionContinuousActionNodeExecutable>();

        private HashSet<BaseMotionNodeExecutable> _nodesWaitingForExecution = new HashSet<BaseMotionNodeExecutable>();

        private HashSet<BaseMotionNodeExecutable> _justFinishedNodes = new HashSet<BaseMotionNodeExecutable>();

        private List<MotionContinuousActionNodeExecutable> _continuousMotionToRemove =
            new List<MotionContinuousActionNodeExecutable>();

        private StartMotionNodeExecutable _startingNode;

        public StartMotionNodeExecutable StartingNode {
            get
            {
                if (_startingNode != null)
                {
                    return _startingNode;
                }
                else
                {
                    int startingNodeCount = 0;
                    foreach (var nodeExecutable in _allNodes)
                    {
                        if (nodeExecutable is StartMotionNodeExecutable startNode)
                        {
                            _startingNode = startNode;
                            startingNodeCount++;
                        }
                    }

                    if (startingNodeCount > 1)
                    {
                        Debug.LogError($"Error! Graph contains more than 1 starting node. " +
                                       $"Behaviour of graph in that case is undefined!");
                    }
                    else if (startingNodeCount == 0)
                    {
                        Debug.LogError($"Error! Graph doesn't contain starting node! ");
                    }

                    return _startingNode;
                }
            }
        }

        public bool InExecution => _justFinishedNodes.Count > 0 || _nodesWaitingForExecution.Count > 0 ||
                                   _activeContinuousNodes.Count > 0;

        public void BuildFromTemplate(CharacterMotionGraph graphTemplate)
        {
            Dictionary<BaseMotionNode, BaseMotionNodeExecutable> templateAssociation =
                new Dictionary<BaseMotionNode, BaseMotionNodeExecutable>();

            foreach (var node in graphTemplate.nodes)
            {
                if (node is BaseMotionNode motionNode)
                {
                    var executable = motionNode.CreateExecutable();
                    _allNodes.Add(executable);
                    templateAssociation.Add(motionNode, executable);
                }
                else
                {
                    Debug.LogError($"Exception node with name {node.name} has Uncorrect type = {node.GetType().Name}");
                    break;
                }
            }

            foreach (var kv in templateAssociation)
            {
                kv.Value.CreateConnections(kv.Key, templateAssociation);
            }
        }

        public void StartExecution(CharacterMotionGraphExecutionContext context)
        {
           ExecuteNode(StartingNode, context);
        }

        public void PropagateExecution(CharacterMotionGraphExecutionContext context)
        {
            var nodesToExecute = new List<BaseMotionNodeExecutable>();
            

            while (_justFinishedNodes.Count > 0)
            {
                nodesToExecute.Clear();
                
                foreach (var finishedNode in _justFinishedNodes)
                {
                    foreach (var nextNode in finishedNode.GetNextNodes())
                    {
                        _nodesWaitingForExecution.Add(nextNode);
                    }
                }
                
                _justFinishedNodes.Clear();
                
                foreach (var node in _nodesWaitingForExecution)
                {
                    if (node.CheckIsExecutionAllowed(context))
                    {
                        nodesToExecute.Add(node);
                    }
                }

                foreach (var node in nodesToExecute)
                {
                    _nodesWaitingForExecution.Remove(node);
                }

                foreach (var node in nodesToExecute)
                {
                    ExecuteNode(node, context);
                }
            }
        }

        public void Tick(CharacterMotionGraphExecutionContext context, float delta)
        {
            _continuousMotionToRemove.Clear();
            
            foreach (var continuousNode in _activeContinuousNodes)
            {
                continuousNode.UpdateExecution(context, delta);

                if (continuousNode.Executed)
                {
                    _justFinishedNodes.Add(continuousNode);
                    _continuousMotionToRemove.Add(continuousNode);
                }
            }

            foreach (var cont in _continuousMotionToRemove)
            {
                _activeContinuousNodes.Remove(cont);
            }
            
            PropagateExecution(context);
        }

        private void ExecuteNode(BaseMotionNodeExecutable node, CharacterMotionGraphExecutionContext context)
        {
            node.Execute(context);

            if (node.Executed)
            {
                _justFinishedNodes.Add(node);
            }
            else if(node is MotionContinuousActionNodeExecutable continuousNode)
            {
                _activeContinuousNodes.Add(continuousNode);
            }
        }

        public void ResetGraphState()
        {
            foreach (var node in _allNodes)
            {
                node.ResetNodeState();
            }
            
            _activeContinuousNodes.Clear();
            _nodesWaitingForExecution.Clear();
        }
    }
}