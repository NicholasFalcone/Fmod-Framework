using UnityEditor;

namespace FmodEditor
{
    [CustomEditor(typeof(BusData))]
    public class BusDataEditor : Editor
    {
        private BusData m_fmodBus;

        private void OnEnable()
        {
            m_fmodBus = (BusData)target;
        }

        public override void OnInspectorGUI()
        {
            m_fmodBus.BusVolume = EditorGUILayout.Slider(m_fmodBus.BusVolume,0,1);
            m_fmodBus.Muted = EditorGUILayout.Toggle(m_fmodBus.Muted);
            EditorUtility.SetDirty(m_fmodBus);
        }

    }
}