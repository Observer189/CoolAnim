using System.Collections;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation
{
    public class CharacterMotionGraphExecutor
    {
        private CharacterMotionGraphExecutionContext _executionContext = new CharacterMotionGraphExecutionContext();

        private Dictionary<CharacterMotionGraph, CharacterMotionGraphExecutable> _graphCache = new Dictionary<CharacterMotionGraph, CharacterMotionGraphExecutable>();

        public void Initialize(CharacterMotionController motionOwner)
        {
            _executionContext.Initialize(motionOwner);
        }

        public IEnumerator ExecuteMotionGraph(CharacterMotionGraph graph)
        {
            CharacterMotionGraphExecutable graphExecutable = null;

            if (_graphCache.TryGetValue(graph, out var executable))
            {
                graphExecutable = executable;
                graphExecutable.ResetGraphState();
            }
            else
            {
                graphExecutable = CompileGraph(graph);
            }
            
            graphExecutable.StartExecution(_executionContext);

            while (graphExecutable.InExecution)
            {
                graphExecutable.Tick(_executionContext, Time.deltaTime);
                
                yield return null;
            }
        }

        public CharacterMotionGraphExecutable CompileGraph(CharacterMotionGraph graph)
        {
            CharacterMotionGraphExecutable graphExecutable = new CharacterMotionGraphExecutable();
            
            graphExecutable.BuildFromTemplate(graph);

            return graphExecutable;
        }
    }
}