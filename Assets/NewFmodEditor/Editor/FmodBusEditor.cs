using UnityEngine;
using UnityEditor;

namespace FmodEditor
{
    [CustomEditor(typeof(FmodBus))]
    public class FmodBusEditor : Editor
    {
        private static FmodBus m_fmodBus;

        private void OnEnable()
        {
            m_fmodBus = (FmodBus)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Init Bus"))
            {
                BuildingBus();
            }
        }

        [MenuItem("FmodEditor/BuildBus")]
        public static void BuildingBus()
        {
            m_fmodBus.Init();
        }

    }
}