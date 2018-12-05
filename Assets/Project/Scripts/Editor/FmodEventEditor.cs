using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FmodEvent))]
[CanEditMultipleObjects]
public class FmodEventEditor : Editor
{
    private string m_oldEventPath;

    private float[] m_sliderValue;
    private FmodEvent m_fmodEVent;

    public void OnEnable()
    {
        m_fmodEVent = (FmodEvent)target;
        InitVariable();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginVertical();

        if (m_oldEventPath != m_fmodEVent.EventPath)
        {
            Debug.Log("Changeing Path");

            InitVariable();
        }

        for (int i = 0; i < m_sliderValue.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_fmodEVent.ParameterInfourn[i].ParameterName);
            m_sliderValue[i] = EditorGUILayout.Slider(m_sliderValue[i], m_fmodEVent.ParameterInfourn[i].MinIndex, m_fmodEVent.ParameterInfourn[i].MaxIndex);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Event"))
        {
            m_fmodEVent.InitFmodEvent();
            InitVariable();
        }

        if (GUILayout.Button("PlayAudio"))
        {
            m_fmodEVent.PlayAudio();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void InitVariable()
    {
        m_fmodEVent.InitFmodEvent();

        m_oldEventPath = m_fmodEVent.EventPath;

        m_sliderValue = new float[m_fmodEVent.ParameterInfourn.Length];
        for (int i = 0; i < m_sliderValue.Length; i++)
        {
            m_sliderValue[i] = m_fmodEVent.ParameterInfourn[i].Value;
        }


    }

}
