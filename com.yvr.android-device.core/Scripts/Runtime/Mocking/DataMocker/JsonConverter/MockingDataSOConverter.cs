using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace YVR.AndroidDevice.Core
{
    public class MockingDataSOConverter : JsonConverter
    {
        private string m_SavingPath = null;
        private string m_RuntimePath = null;

        public MockingDataSOConverter(string savingPath, string runtimePath)
        {
            m_SavingPath = savingPath;
            m_RuntimePath = runtimePath;
        }

        public MockingDataSOConverter() { }

        public override bool CanConvert(System.Type objectType)
        {
            return typeof(MockingDataSO).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            string filePath = (string) reader.Value;
            if (filePath == null) return null;

            string json = File.ReadAllText(filePath);
            JObject jObject = JObject.Parse(json);
            var type = System.Type.GetType(jObject["Type"].ToString());
            var ret = ScriptableObject.CreateInstance(type);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new JsonConverter[] {new TextureConverter(m_SavingPath,m_RuntimePath)}
            };
            serializer = JsonSerializer.Create(settings);
            serializer.Populate(jObject.CreateReader(), ret);
            return ret;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not MockingDataSO mockingDataSO) return;

            string fileName = $"{mockingDataSO.GetType().Name}_{mockingDataSO.name}.json";
            string savingFilePath = Path.Combine(m_SavingPath, fileName);
            string runtimeFilePath = Path.Combine(m_RuntimePath, fileName);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new JsonConverter[] {new TextureConverter(m_SavingPath,m_RuntimePath)}
            };

            JObject jObject = JObject.FromObject(mockingDataSO, JsonSerializer.Create(settings));
            jObject["Type"] = mockingDataSO.GetType().ToString();

            File.WriteAllText(savingFilePath, jObject.ToString(Formatting.Indented));
            writer.WriteValue(runtimeFilePath);
        }
    }
}