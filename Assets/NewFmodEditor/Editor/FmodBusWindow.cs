using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FmodEditor
{
    public class FmodBusWindow : EditorWindow
    {
        private FmodBus m_fmodBus;
        private bool canEdit = true;
        public static String m_datapath;
        private Vector2 scrollPos;
        private const string dataPathPref = "DataPath";

        private const string m_fileName = "BusData.asset";

        private void OnEnable()
        {
            ///Check if a bus data exist and take her data
            string[] currentFile = AssetDatabase.FindAssets("t:" + typeof(FmodBus).FullName);

            if (currentFile.Length > 0)
            {
                ///Take the first element path found
                string currentPath = AssetDatabase.GUIDToAssetPath(currentFile[0]);
                ///take the data reference
                m_fmodBus = AssetDatabase.LoadAssetAtPath<FmodBus>(currentPath);
                ///Update path with current
                m_datapath = currentPath.Replace(m_fileName, "");
                ///update and Save playerpref
                PlayerPrefs.SetString(dataPathPref, m_datapath);
                PlayerPrefs.Save();
                ///Initialize BusData
                // m_fmodBus.Init(m_datapath);
                EditorUtility.SetDirty(m_fmodBus);

            }
            else
            {
                m_datapath = PlayerPrefs.GetString(dataPathPref, "");
                m_fmodBus = null;
            }
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
                canEdit = false;

            canEdit = EditorGUILayout.BeginToggleGroup("Can Edit", canEdit);

            //Create BusData and print on window

            ///If data path is null set enable the create path button
            if (m_datapath == null || m_datapath == "")
            {
                if (GUILayout.Button("Create Path"))
                {
                    SetPath();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Data Path: " + m_datapath.ToString());
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Change Path"))
                {
                    ChangePath();
                }

                if (GUILayout.Button("Create BusData"))
                {
                    Init();
                }
                EditorGUILayout.EndHorizontal();
            }

            if (m_fmodBus == null)
                return;
            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos, GUILayout.Width(400), GUILayout.Height(500)))
            {
                GUILayout.FlexibleSpace();
                scrollPos = scrollView.scrollPosition;
                foreach (BusData data in m_fmodBus.busData)
                {
                    EditorGUILayout.TextArea(data.BusName);
                    ShowBusInfo(data);
                }
            }

            EditorGUILayout.EndToggleGroup();
        }

        ///Called to create and show bus data
        private void Init()
        {

            if (m_fmodBus == null)
            {
                if (AssetDatabase.IsValidFolder(m_datapath))
                {
                    m_fmodBus = ScriptableObject.CreateInstance<FmodBus>();
                    string _uniquePath = AssetDatabase.GenerateUniqueAssetPath(m_datapath + "/" + m_fileName);
                    AssetDatabase.CreateAsset(m_fmodBus, _uniquePath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogError("Path dosen't exist");
                }
                m_fmodBus.Init(m_datapath);
            }
            else
            {
                m_fmodBus.Init(m_datapath);
            }
        }

        ///Called to show on editor all bus data
        private void ShowBusInfo(BusData data)
        {
            data.BusVolume = EditorGUILayout.Slider("Volume:", data.BusVolume, 0, 1);
            data.Muted = EditorGUILayout.Toggle("Muted", data.Muted);
        }

        ///Called to open explorer folder
        private void SetPath()
        {
            string path = EditorUtility.OpenFolderPanel("Chose FmodBusData", Application.dataPath, "FmodBusData");
            m_datapath = path;
            CleanPath();
            PlayerPrefs.SetString(dataPathPref, path);
            PlayerPrefs.Save();
        }

        ///Called to update current path data
        private void ChangePath()
        {
            SetPath();
            if (m_fmodBus != null)
                ChangeFileDirectory();
        }

        ///Called to chane the bus data folder
        private void ChangeFileDirectory()
        {
            AssetDatabase.ValidateMoveAsset(AssetDatabase.GetAssetPath(m_fmodBus), m_datapath);
        }

        ///Clean a path chacking difference bethween application path and envoiroment path
        private void CleanPath()
        {
            string difference = m_datapath.Replace(Application.dataPath, "");
            m_datapath = "Assets" + difference;
        }

        ///Method called on editor to open this window
        [MenuItem("FmodEditor/BusEditor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(FmodBusWindow));
        }

    }
}
