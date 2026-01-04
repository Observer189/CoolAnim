using System.Collections;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace CoolAnimation
{
    public class CharacterMotionGraphExecutor
    {
        private CharacterMotionGraphExecutionContext _executionContext = new CharacterMotionGraphExecutionContext();

        private Dictionary<CharacterMotionGraphNew, CharacterMotionGraphExecutableNew> _graphCache = new Dictionary<CharacterMotionGraphNew, CharacterMotionGraphExecutableNew>();

        public void Initialize(CharacterMotionController motionOwner)
        {
            _executionContext.Initialize(motionOwner);
        }

        public IEnumerator ExecuteMotionGraph(CharacterMotionGraphNew graph)
        {
            CharacterMotionGraphExecutableNew graphExecutable = null;

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

        public CharacterMotionGraphExecutableNew CompileGraph(CharacterMotionGraphNew graph)
        {
            CharacterMotionGraphExecutableNew graphExecutable = new CharacterMotionGraphExecutableNew();
            
            graphExecutable.BuildFromTemplate(graph);

            return graphExecutable;
        }
    }
}