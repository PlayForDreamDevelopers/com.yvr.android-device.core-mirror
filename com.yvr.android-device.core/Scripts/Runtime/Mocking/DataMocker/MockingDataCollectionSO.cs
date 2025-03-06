using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using YVR.Utilities;
using Debug = UnityEngine.Debug;

#pragma warning disable CS8321 // Local function is declared but never used

namespace YVR.AndroidDevice.Core
{
    /// <summary>
    /// This class represents a collection of mocking data entries.
    /// </summary>
    [CreateAssetMenu(menuName = "YVR/Android-Device/MockingDataCollection")]
    public class MockingDataCollectionSO : ScriptableObject
    {
        private static Dictionary<string, MockingDataCollectionSO> s_Path2CollectionSODict = new();

        /// <summary>
        /// An array of MockingDataEntry objects. Each entry represents a key-value pair of mocking data.
        /// </summary>
        public MockingDataEntry[] entries;

        /// <summary>
        /// The path at target device where the mocking data will be pushed to.
        /// When the Push button in the MockingDataCollectionSO inspector is clicked, the mocking data will be pushed to this path.
        /// </summary>
        public string targetPushPath = null;

        /// <summary>
        /// Tries to get the value of the MockingDataSO associated with a specific key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="value">The value data of the MockingDataSO associated with the key.</param>
        /// <returns>True if the key was found and the value could be retrieved, false otherwise.</returns> 
        public bool GetMockingDataValue<T>(string key, out T value)
        {
            MockingDataEntry entry = entries.FirstOrDefault(dataEntry => dataEntry.key == key);
            if (entry.value == null) throw new Exception($"Failed to get entry for {key}");

            value = default;
            bool succeed = GetMockingData(key, out MockingDataSO data) &&
                           data.TryGetFieldValue("value", out value);

            if (!succeed) Debug.LogError($"Failed to get value from MockingDataSO for key {key}");

            return succeed;
        }

        /// <summary>
        /// Tries to get a MockingDataSO associated with a specific key.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="data">The MockingDataSO associated with the key.</param>
        /// <returns>True if the key was found and the MockingDataSO could be retrieved, false otherwise.</returns>
        public bool GetMockingData(string key, out MockingDataSO data)
        {
            MockingDataEntry entry = entries.FirstOrDefault(data => data.key == key);
            data = entry.value;
            return data != null;
        }


        /// <summary>
        /// Loads a MockingDataCollectionSO from a specified path.
        /// </summary>
        /// <param name="path">The path to load the MockingDataCollectionSO from.</param>
        /// <returns>The loaded MockingDataCollectionSO.</returns>
        public static MockingDataCollectionSO Load(string path)
        {
            if (s_Path2CollectionSODict.TryGetValue(path, out MockingDataCollectionSO value))
                return value;
#if UNITY_EDITOR
            var dataCollection = UnityEditor.AssetDatabase.LoadAssetAtPath<MockingDataCollectionSO>(path);
#else
            var dataCollection = LoadAtRuntime();

#endif

            s_Path2CollectionSODict.SafeAdd(path, dataCollection);

            return dataCollection;

            MockingDataCollectionSO LoadAtRuntime()
            {
                string jsonData = File.ReadAllText(path);

                var ret = CreateInstance<MockingDataCollectionSO>();
                JsonConvert.PopulateObject(jsonData, ret, new JsonSerializerSettings()
                {
                    Converters = new JsonConverter[] {new MockingDataSOConverter()}
                });
                return ret;
            }
        }
    }
}