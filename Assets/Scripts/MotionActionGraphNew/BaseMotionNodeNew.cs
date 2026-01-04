using System;
using System.Collections.Generic;
using CoolAnimation;
using UnityEngine;
using XNode;

namespace CoolAnimation
{
    [Serializable]
    public abstract class BaseMotionNodeNew : Node
    {
        [SerializeField] protected string _name;

        public abstract BaseMotionNodeExecutableNew CreateExecutable();
    }
}

public abstract class BaseMotionNodeExecutableNew
{
    protected List<BaseMotionNodeExecutableNew> _parents;

    protected List<BaseMotionNodeExecutableNew> _children;

    protected bool _executed;

    public bool Executed => _executed;

    public virtual bool CheckIsExecutionAllowed(CharacterMotionGraphExecutionContext context)
    {
        foreach (var parent in _parents)
        {
            if (!parent._executed)
            {
                return false;
            }
        }

        return true;
    }

    public virtual void Execute(CharacterMotionGraphExecutionContext context)
    {
        _executed = true;
    }

    public virtual List<BaseMotionNodeExecutableNew> GetNextNodes()
    {
        return _children;
    }

    public virtual void ResetNodeState()
    {
        _executed = false;
    }

    public virtual void CreateConnections(BaseMotionNodeNew ownTemplate,
        Dictionary<BaseMotionNodeNew, BaseMotionNodeExecutableNew> templateAssociations)
    {
        _parents = new List<BaseMotionNodeExecutableNew>();
        foreach (var inputPort in ownTemplate.Inputs)
        {
            if (!inputPort.IsConnected)
            {
                continue;
            }
            
            if (inputPort.Connection.node is BaseMotionNodeNew inputMotionNode)
            {
                if (templateAssociations.TryGetValue(inputMotionNode, out var inputNodeExec))
                {
                    _parents.Add(inputNodeExec);
                }
            }
        }

        _children = new List<BaseMotionNodeExecutableNew>();
        foreach (var outputPort in ownTemplate.Outputs)
        {
            if (!outputPort.IsConnected)
            {
                continue;
            }

            if (outputPort.Connection.node is BaseMotionNodeNew inputMotionNode)
            {
                if (templateAssociations.TryGetValue(inputMotionNode, out var outputNodeExec))
                {
                    _children.Add(outputNodeExec);
                }
            }
        }
    }
}