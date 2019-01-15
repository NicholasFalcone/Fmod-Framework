using UnityEngine;
using UnityEditor;

namespace FmodEditor
{
    [CustomEditor(typeof(FmodEvent))]
    [CanEditMultipleObjects]
    public class FmodEventEditor : Editor
    {
        /// Target class of editor
        private FmodEvent m_fmodEVent;
        /// Used to chack if the eventpath is changed
        private string m_oldEventPath;

        private bool canEdit;

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

            if (Application.isPlaying)
                canEdit = false;

            canEdit = EditorGUILayout.BeginToggleGroup("Can Edit", canEdit);

            ///Check if event path is changed
            if (m_oldEventPath != m_fmodEVent.EventPath)
                InitVariable();

            DrawDefaultInspector();
            /*0*/
            EditorGUILayout.BeginVertical();

            EditorGUILayout.EnumPopup("Sound Type:", m_fmodEVent.SoundType);
            if (m_fmodEVent.Is3d)
            {
                EditorGUILayout.FloatField("Max Distance:", m_fmodEVent.MaxDistance);
                EditorGUILayout.FloatField("Min Distance:", m_fmodEVent.MinDistance);
            }

            m_fmodEVent.Volume = EditorGUILayout.Slider("Volume: ", m_fmodEVent.Volume, 0, m_fmodEVent.MaxVolume);

            EditorGUILayout.FloatField("Current Number of instance:", m_fmodEVent.NumberOfInstance);


            m_fmodEVent.RenameFile = EditorGUILayout.Toggle("Rename your FmodEvent?", m_fmodEVent.RenameFile);

            EditorGUILayout.Toggle("Has Cue", m_fmodEVent.HasCue);





            EditorGUILayout.Space();
            if (m_fmodEVent.EventPath != "")
            {
                ///Create a slider for all parameter
                ShowParameterSlider();
            }
            EditorGUILayout.Space();

            #region ButtonEditor
            /*1*/
            EditorGUILayout.BeginHorizontal();
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
                if (GUILayout.Button("TriggerCue", GUILayout.Width(80), GUILayout.Height(80)))
                {
                    m_fmodEVent.TriggerCue();
                }
            }
            #endregion


            /*0*/
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndToggleGroup();
        }

        private void RenameFile()
        {
            string[] path = m_fmodEVent.EventPath.Split('/');

            if (m_fmodEVent.name == path[path.Length - 1])
                return;

            string assetPath = AssetDatabase.GetAssetPath(m_fmodEVent);
            Debug.Log(assetPath);
            AssetDatabase.RenameAsset(assetPath, path[path.Length - 1]);
            AssetDatabase.SaveAssets();
        }


        private void ShowParameterSlider()
        {
            if (m_fmodEVent.ParameterInfo.Length == 0)
            {
                EditorGUILayout.LabelField("This event has no Parameter");
                return;
            }

            for (int i = 0; i < m_fmodEVent.ParameterInfo.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(m_fmodEVent.ParameterInfo[i].ParameterName);
                m_fmodEVent.ParameterInfo[i].Value = EditorGUILayout.Slider(m_fmodEVent.ParameterInfo[i].Value, m_fmodEVent.ParameterInfo[i].MinIndex, m_fmodEVent.ParameterInfo[i].MaxIndex);
                EditorGUILayout.EndHorizontal();
                EditorUtility.SetDirty(m_fmodEVent);
            }
        }

        private void InitVariable()
        {
            if (m_fmodEVent.RenameFile)
                RenameFile();
            if (!Application.isPlaying)
                m_fmodEVent.StopAudio();

            m_fmodEVent.InitFmodEvent();

            m_oldEventPath = m_fmodEVent.EventPath;

        }

    }
}