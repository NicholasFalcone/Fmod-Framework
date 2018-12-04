using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FmodEvent))]
[CanEditMultipleObjects]
public class FmodEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FmodEvent fmodEVent = (FmodEvent)target;

        if (GUILayout.Button("Build Event"))
        {
            fmodEVent.InitFmodEvent();
        }


    }
}
