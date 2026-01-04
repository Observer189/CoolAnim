using GraphProcessor;
using UnityEngine;

namespace CoolAnimation
{
    public abstract class MotionDataNode<T> : BaseMotionNode
    {
        [Output(name = "Data", allowMultiple = true), SerializeField]
        protected T _data;
    }
    
    public abstract class MotionDataNodeExecutable<T> : BaseMotionNodeExecutable
    {
        protected T _data;

        public virtual void FillData(T data)
        {
            _data = data;
        }

        public virtual T GetData()
        {
            return _data;
        }
    }
}