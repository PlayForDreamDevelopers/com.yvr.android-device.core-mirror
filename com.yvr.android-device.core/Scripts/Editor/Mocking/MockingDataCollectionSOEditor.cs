#if USE_YVR_JSON_PARSER
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using YVR.JsonParser;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core.Editor
{
    [CustomEditor(typeof(MockingDataCollectionSO))]
    public class MockingDataCollectionSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var mockingDataCollection = (MockingDataCollectionSO) target;

            if (GUILayout.Button("Push"))
            {
                PublishMockingData2Device(mockingDataCollection);
            }
        }

        private void PublishMockingData2Device(MockingDataCollectionSO mockingDataCollection)
        {
            string targetPath = mockingDataCollection.targetPushPath;

            if (!AdbHelper.GetFirstConnectedDevice(out string device))
                throw new Exception("Cannot push mocking data, for no connected device");

            if (!AdbHelper.IsDeviceRoot())
                throw new Exception("Cannot push mocking data, for device is no-rooted");

            string savePath = ConvertingMockingDataToJson();

            if (AdbHelper.IsPathExist(targetPath))
                AdbHelper.RemoveFolder(targetPath);
            else
                AdbHelper.CreateFolder(targetPath);

            AdbHelper.Push(savePath, targetPath);

            this.Info($"Mocking Data has been pushed to {device}, at path {targetPath}");

            // Convert the mocking data to json, and saved in pc temp folder
            string ConvertingMockingDataToJson()
            {
                string folder = Path.Join(Application.temporaryCachePath,
                                          $"/MockingData/{mockingDataCollection.name}/");
                Debug.Log($"Serialized mocking data saved to {folder}");
                if (Directory.Exists(folder))
                    Directory.Delete(folder, true);
                Directory.CreateDirectory(folder);

                // The data will be saved in the folder while doing serialization
                JsonParserMgr.SerializeObject(mockingDataCollection, new JsonSerializerSettings
                {
                    Converters = new JsonConverter[] {new MockingDataCollectionSOConverter(folder)}
                });

                return folder;
            }
        }
    }
}
#endif