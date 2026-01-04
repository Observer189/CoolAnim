using CoolAnimation;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterMotionGraph))]
public class MotionActionGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Graph Editor", GUILayout.Height(50), GUILayout.ExpandWidth(true)))
        {
            MotionActionGraphWindow.Open();
        }
    }
}