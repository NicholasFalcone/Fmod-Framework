using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FmodEvent))]
[CanEditMultipleObjects]
public class FmodEventEditor : Editor
{
    /// Target class of editor
    private FmodEvent m_fmodEVent;
    /// Used to chack if the eventpath is changed
    private string m_oldEventPath;
    /// Slider value of parameter value
    private float[] m_sliderValue;

    public void OnEnable()
    {
        m_fmodEVent = (FmodEvent)target;
        if (m_fmodEVent.EventPath == "")
            return;
        m_oldEventPath = m_fmodEVent.EventPath;

        InitVariable();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        /*0*/EditorGUILayout.BeginVertical();

        m_fmodEVent.RenameFile = EditorGUILayout.Toggle("Do you wanna rename your FmodEvent?", m_fmodEVent.RenameFile);



        EditorGUILayout.Toggle("Has Cue", m_fmodEVent.HasCue);

        ///Check if event path is changed
        if (m_oldEventPath != m_fmodEVent.EventPath)
            InitVariable();
        EditorGUILayout.Space();
        if (m_fmodEVent.EventPath != "")
        {
            ///Create a slider for all parameter
            ShowParameterSlider();
        }


        EditorGUILayout.Space();

        /*1*/EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Event"))
        {

            m_fmodEVent.InitFmodEvent();
            InitVariable();
            m_fmodEVent.StopAudio();
        }

        if (GUILayout.Button("PlayAudio"))
        {
            m_fmodEVent.PlayAudio();
            EditorUtility.SetDirty(m_fmodEVent);
        }

        if (GUILayout.Button("StopAudio"))
        {
            m_fmodEVent.StopAudio();
        }
        /*1*/
        EditorGUILayout.EndHorizontal();

        if (m_fmodEVent.HasCue)
        {
            if(GUILayout.Button("TriggerCue", GUILayout.Width(80), GUILayout.Height(80)))
            {
                m_fmodEVent.TriggerCue();
            }
        }


        /*0*/EditorGUILayout.EndVertical();
    }

    private void RenameFile()
    {
        string[] path = m_fmodEVent.EventPath.Split('/');

        if (m_fmodEVent.name == path[path.Length - 1])
            return;

        string assetPath = AssetDatabase.GetAssetPath(m_fmodEVent);
        Debug.Log(assetPath);
        AssetDatabase.RenameAsset(assetPath, path[path.Length-1]);
        AssetDatabase.SaveAssets();
    }


    private void ShowParameterSlider()
    {
        if (m_sliderValue.Length == 0)
        {
            EditorGUILayout.LabelField("This event has no Parameter");
            return;
        }

        for (int i = 0; i < m_sliderValue.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(m_fmodEVent.ParameterInfo[i].ParameterName);
            m_sliderValue[i] = EditorGUILayout.Slider(m_fmodEVent.ParameterInfo[i].Value, m_fmodEVent.ParameterInfo[i].MinIndex, m_fmodEVent.ParameterInfo[i].MaxIndex);
            EditorGUILayout.EndHorizontal();
            if(!Application.isPlaying)
            {
                m_fmodEVent.ChangeParameter(i, m_sliderValue[i]);
            }
            EditorUtility.SetDirty(m_fmodEVent);
        }
    }

    private void InitVariable()
    {
        if (m_fmodEVent.RenameFile)
            RenameFile();
        if(!Application.isPlaying)
            m_fmodEVent.StopAudio();

        m_fmodEVent.InitFmodEvent();

        m_oldEventPath = m_fmodEVent.EventPath;

        m_sliderValue = new float[m_fmodEVent.ParameterInfo.Length];
        for (int i = 0; i < m_sliderValue.Length; i++)
        {
            m_sliderValue[i] = m_fmodEVent.ParameterInfo[i].Value;
        }

    }

}
