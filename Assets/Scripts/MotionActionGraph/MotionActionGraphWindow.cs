using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace CoolAnimation
{


    public class MotionActionGraphWindow : BaseGraphWindow
    {
        // Add the window in the editor menu
        [MenuItem("Window/MotionActionGraph")]
        public static BaseGraphWindow Open()
        {
            var graphWindow = GetWindow<MotionActionGraphWindow>();

            graphWindow.Show();
            if (Selection.activeObject is CharacterMotionGraph assetGraph)
            {
                graphWindow.InitializeGraph(assetGraph);
            }

            return graphWindow;
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            // Set the window title
            titleContent = new GUIContent("Default Graph");

            // Here you can use the default BaseGraphView or a custom one (see section below)
            var graphView = new BaseGraphView(this);

            rootView.Add(graphView);
        }
    }
}
