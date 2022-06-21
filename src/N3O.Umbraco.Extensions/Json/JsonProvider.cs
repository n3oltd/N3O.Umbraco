using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N3O.Umbraco.Json;

public class JsonProvider : IJsonProvider {
    private readonly IReadOnlyList<JsonConverter> _jsonConverters;

    public JsonProvider(IEnumerable<JsonConverter> jsonConverters) {
        _jsonConverters = jsonConverters.ToList();
    }

    public void ApplySettings(JsonSerializerSettings serializerOptions) {
        serializerOptions.ContractResolver = new JsonContractResolver();
        serializerOptions.Formatting = Formatting.Indented;
        serializerOptions.NullValueHandling = NullValueHandling.Include;

        serializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        _jsonConverters.Do(c => serializerOptions.Converters.Add(c));
    }

    public object DeserializeDynamic(string json) {
        var settings = GetSettings();

        return JsonConvert.DeserializeObject(json, settings);
    }

    public async Task<object> DeserializeDynamicAsync(Stream utf8Stream) {
        using (var reader = new StreamReader(utf8Stream, Encoding.UTF8)) {
            var json = await reader.ReadToEndAsync();

            return DeserializeDynamic(json);
        }
    }

    public async Task<object> DeserializeDynamicAsync(Task<Stream> asyncUtf8Stream) {
        var utf8Stream = await asyncUtf8Stream;

        var obj = await DeserializeDynamicAsync(utf8Stream);

        return obj;
    }

    public T DeserializeObject<T>(string json) {
        return (T)DeserializeObject(json, typeof(T));
    }

    public object DeserializeObject(string json, Type type) {
        var settings = GetSettings();

        return JsonConvert.DeserializeObject(json, type, settings);
    }

    public async Task<T> DeserializeObjectAsync<T>(Stream utf8Stream) {
        var result = (T)await DeserializeObjectAsync(utf8Stream, typeof(T));

        return result;
    }

    public async Task<object> DeserializeObjectAsync(Stream utf8Stream, Type type) {
        using (var reader = new StreamReader(utf8Stream, Encoding.UTF8)) {
            var json = await reader.ReadToEndAsync();

            return DeserializeObject(json, type);
        }
    }

    public async Task<T> DeserializeObjectAsync<T>(Task<Stream> asyncUtf8Stream) {
        var utf8Stream = await asyncUtf8Stream;

        var obj = await DeserializeObjectAsync<T>(utf8Stream);

        return obj;
    }

    public async Task<object> DeserializeObjectAsync(Task<Stream> asyncUtf8Stream, Type type) {
        var utf8Stream = await asyncUtf8Stream;

        var obj = await DeserializeObjectAsync(utf8Stream, type);

        return obj;
    }

    public JsonSerializerSettings GetSettings() {
        var settings = new JsonSerializerSettings();

        ApplySettings(settings);

        return settings;
    }

    public string SerializeObject(object value, Formatting formatting = Formatting.Indented) {
        var settings = GetSettings();

        settings.Formatting = formatting;

        return JsonConvert.SerializeObject(value, settings);
    }

    public void SerializeObject(JsonWriter writer, object value, Formatting formatting = Formatting.Indented) {
        var settings = GetSettings();

        settings.Formatting = formatting;

        var serializer = JsonSerializer.Create(settings);

        serializer.Serialize(writer, value);
    }
}
