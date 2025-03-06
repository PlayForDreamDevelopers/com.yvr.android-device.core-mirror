using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    public class TextureConverter : JsonConverter
    {
        private string m_SavingPath = null;
        private string m_RuntimePath = null;

        public TextureConverter(string savingPath, string runtimePath)
        {
            m_SavingPath = savingPath;
            m_RuntimePath = runtimePath;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
#if UNITY_EDITOR
            if (value is not Texture texture) return;
            string path = Path.GetFullPath(AssetDatabase.GetAssetPath(texture));
            string savingPath = Path.Combine(m_SavingPath, texture.name + ".png");
            string runtimePath = Path.Combine(m_RuntimePath, texture.name + ".png");
            File.Copy(path, savingPath);
            writer.WriteValue(runtimePath);
#endif
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            string filePath = (string) reader.Value;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Debug.Log($"Can not deserialize texture for no path from file path {filePath}");
                return null;
            }

            byte[] data = File.ReadAllBytes(filePath);
            var texture = new Texture2D(2, 2);
            if (texture.LoadImage(data)) return texture;

            Debug.Log($"Can not deserialize texture for wrong data from file path {filePath}");
            return null;
        }

        public override bool CanConvert(Type objectType) { return typeof(Texture).IsAssignableFrom(objectType); }
    }
}