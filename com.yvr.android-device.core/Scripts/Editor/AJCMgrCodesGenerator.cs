#if USE_YVR_TOOLS
using System.IO;
using UnityEditor;
using UnityEngine;
using YVR.Tools;

namespace YVR.AndroidDevice.Core.Editor
{
    public class AJCMgrCodesGenerator : EditorWindow
    {
        [SerializeField] private string m_Namespace = "YVR";
        [SerializeField] private string m_Name = "Foo";
        private string m_SelectedPath = "Assets";

        [MenuItem("Assets/Create/YVR/Android-Device/Generate AJCMgr Codes &%#m", false, 70)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(AJCMgrCodesGenerator));
            window.titleContent = new GUIContent("Generate AJCMgr Codes");
            window.Show();
        }

        private void OnEnable() { m_SelectedPath = CodesGeneratorUtils.GetFirstSelectedObjPath(); }

        private void OnGUI()
        {
            GUILayout.Label("Generate AJCMgr related Files", EditorStyles.boldLabel);
            m_Namespace = EditorGUILayout.TextField("Namespace", m_Namespace);
            m_Name = EditorGUILayout.TextField("AJC Name", m_Name);

            if ((!Event.current.isKey || Event.current.keyCode != KeyCode.Return) && !GUILayout.Button("Create Script"))
                return;

            GenerateArchRelatedClasses();
            AssetDatabase.Refresh();
            Close();
        }

        private void GenerateArchRelatedClasses()
        {
            CreateFile(m_Name + "Mgr.cs", GetFileContent("AJCMgr.txt"));
            CreateFile(m_Name + "Elements.cs", GetFileContent("Elements.txt"));
            CreateFile(m_Name + "Mocker.cs", GetFileContent("Mocker.txt"));

            void CreateFile(string fileName, string content)
            {
                File.WriteAllText($"{m_SelectedPath}/" + fileName, content);
            }

            string GetFileContent(string filePath)
            {
                return CodesGeneratorUtils.GetTemplateText("com.yvr.android-device.core", filePath)
                                          .FormatNamespace(m_Namespace)
                                          .FormatName(m_Name);
            }
        }
    }
}
#endif
