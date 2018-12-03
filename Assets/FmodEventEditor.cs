using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericEvent))]
[CanEditMultipleObjects]
public class FmodEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        base.OnInspectorGUI();
    }
}
