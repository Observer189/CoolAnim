using System;
using System.Collections.Generic;
using CoolAnimation;
using GraphProcessor;
using UnityEngine;

[Serializable]
public abstract class BaseMotionNode : BaseNode
{

    [SerializeField] protected string _name;

    public abstract BaseMotionNodeExecutable CreateExecutable();
}

public abstract class BaseMotionNodeExecutable
{
    protected List<BaseMotionNodeExecutable> _parents;

    protected List<BaseMotionNodeExecutable> _children;

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

    public virtual List<BaseMotionNodeExecutable> GetNextNodes()
    {
        return _children;
    }

    public virtual void ResetNodeState()
    {
        _executed = false;
    }

    public virtual void CreateConnections(BaseMotionNode ownTemplate,
        Dictionary<BaseMotionNode, BaseMotionNodeExecutable> templateAssociations)
    {
        _parents = new List<BaseMotionNodeExecutable>();
        foreach (var inputNode in ownTemplate.GetInputNodes())
        {
            if (inputNode is BaseMotionNode inputMotionNode)
            {
                if (templateAssociations.TryGetValue(inputMotionNode, out var inputNodeExec))
                {
                    _parents.Add(inputNodeExec);
                }
            }
        }

        _children = new List<BaseMotionNodeExecutable>();
        foreach (var outputNode in ownTemplate.GetOutputNodes())
        {
            if (outputNode is BaseMotionNode inputMotionNode)
            {
                if (templateAssociations.TryGetValue(inputMotionNode, out var outputNodeExec))
                {
                    _children.Add(outputNodeExec);
                }
            }
        }
    }
}
