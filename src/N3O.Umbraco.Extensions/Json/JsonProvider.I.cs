using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Json {
    public interface IJsonProvider {
        void ApplySettings(JsonSerializerSettings settings);
        object DeserializeDynamic(string json);
        Task<object> DeserializeDynamicAsync(Stream utf8Stream);
        Task<object> DeserializeDynamicAsync(Task<Stream> asyncUtf8Stream);
        T DeserializeObject<T>(string json);
        object DeserializeObject(string json, Type type);
        Task<T> DeserializeObjectAsync<T>(Stream utf8Stream);
        Task<object> DeserializeObjectAsync(Stream utf8Stream, Type type);
        Task<T> DeserializeObjectAsync<T>(Task<Stream> asyncUtf8Stream);
        Task<object> DeserializeObjectAsync(Task<Stream> asyncUtf8Stream, Type type);
        JsonSerializerSettings GetSettings();
        string SerializeObject(object value, Formatting formatting = Formatting.Indented);
        void SerializeObject(JsonWriter writer, object value, Formatting formatting = Formatting.Indented);
    }
}
