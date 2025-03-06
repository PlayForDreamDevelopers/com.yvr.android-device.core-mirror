using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core.Editor
{
    public class AddMockingDataToCollectionEditor : EditorWindow
    {
        private static MockingDataCollectionSO s_Collection = null;

        [MenuItem("Assets/AddToCollection", true)]
        private static bool AddToCollectionValidation()
        {
            return Selection.objects.Any(o => o is MockingDataSO || o.GetType().IsSubclassOf(typeof(MockingDataSO)));
        }

        [MenuItem("Assets/YVR/Android-Device/MockingData/Add2Collection", false)]
        private static void AddToCollection()
        {
            var target = Selection.activeObject as ScriptableObject;
            if (target == null || !(target is MockingDataSO || target.GetType().IsSubclassOf(typeof(MockingDataSO))))
            {
                Debug.LogWarning("Please select MockingDataSO or its derivatives.");
                return;
            }

            var window = (AddMockingDataToCollectionEditor) GetWindow(typeof(AddMockingDataToCollectionEditor), true,
                                                                      "Select Collection");
            window.ShowPopup();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select the MockingDataCollectionSO instance:");
            Type type = typeof(MockingDataCollectionSO);
            s_Collection = EditorGUILayout.ObjectField("Collection", s_Collection, type, false)
                as MockingDataCollectionSO;

            if (!GUILayout.Button("Add Selected MockingDataSO to Collection")) return;

            if (s_Collection == null) throw new Exception("No MockingDataCollectionSO selected.");

            AddSelectedToCollection();

            EditorUtility.SetDirty(s_Collection);
            AssetDatabase.SaveAssets();
            Close();
        }

        private void AddSelectedToCollection()
        {
            var entriesList = new List<MockingDataEntry>(s_Collection.entries);

            Selection.objects.Where(select => select.GetType().IsSubclassOf(typeof(MockingDataSO))).ForEach(select =>
            {
                var newEntry = new MockingDataEntry
                {
                    key = select.name,
                    value = select as MockingDataSO
                };

                if (entriesList.All(e => e.key != newEntry.key))
                    entriesList.Add(newEntry);
                else
                    Debug.LogWarning($"Key {newEntry.key} already exists in the collection.");
            });

            s_Collection.entries = entriesList.ToArray();
        }
    }
}