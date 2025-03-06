using System;
using System.IO;
using Newtonsoft.Json;

namespace YVR.AndroidDevice.Core
{
    public class MockingDataCollectionSOConverter : JsonConverter
    {
        private string m_SavingPath = null;
        public MockingDataCollectionSOConverter(string savingPath) { m_SavingPath = savingPath; }

        public override bool CanConvert(Type objectType)
        {
            return typeof(MockingDataCollectionSO).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not MockingDataCollectionSO collection) return;

            Directory.CreateDirectory(m_SavingPath);

            string collectionJson = JsonConvert.SerializeObject(collection, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new JsonConverter[] {new MockingDataSOConverter(m_SavingPath, collection.targetPushPath)}
            });

            string collectionFilePath = Path.Combine(m_SavingPath, "collection.json");
            File.WriteAllText(collectionFilePath, collectionJson);
            writer.WriteValue(collectionFilePath);
        }
    }
}