using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FmodEvent))]
[CanEditMultipleObjects]
public class FmodEventEditor : Editor
{
    float scale;
    FmodEvent fmodEVent;
    public void OnEnable()
    {
        fmodEVent = (FmodEvent)target;
        scale = fmodEVent.ParameterInfourn[0].Value;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        scale = EditorGUILayout.Slider(scale, fmodEVent.ParameterInfourn[0].MinIndex, fmodEVent.ParameterInfourn[0].MaxIndex);
        
        if (GUILayout.Button("Build Event"))
        {
            fmodEVent.InitFmodEvent();
        }

    }
}
