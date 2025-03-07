#if USE_YVR_UNIRX
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using YVR.Utilities;

namespace YVR.AndroidDevice.Core.Editor
{
    public static class TextureToMockingDataConverter
    {
        [MenuItem("Assets/YVR/Android-Device/MockingData/Convert2MockingData", true)]
        private static bool ConvertTexturesToMockingDataValidation()
        {
            return Selection.objects.OfType<Texture>().Any();
        }

        [MenuItem("Assets/YVR/Android-Device/MockingData/Convert2MockingData", false)]
        private static void ConvertTexturesToMockingData()
        {
            Selection.objects.ForEach(select =>
            {
                string assetPath = AssetDatabase.GetAssetPath(select);
                if (select is Texture texture)
                {
                    CreateMockingTextureSO(assetPath, texture);
                }
            });
        }

        private static void CreateMockingTextureSO(string assetPath, Texture texture)
        {
            string directoryPath = Path.GetDirectoryName(assetPath);
            string textureFileName = Path.GetFileNameWithoutExtension(assetPath);
            string mockingDataPath = Path.Combine(directoryPath!, textureFileName + ".asset");

            if (File.Exists(mockingDataPath))
            {
                Debug.LogWarning($"MockingTextureSO for {textureFileName} already exists.");
                return;
            }

            var mockingTextureSO = ScriptableObject.CreateInstance<MockingTextureSO>();
            mockingTextureSO.value = texture;

            AssetDatabase.CreateAsset(mockingTextureSO, mockingDataPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"Created MockingTextureSO for {textureFileName}.");
        }
    }
}
#endif