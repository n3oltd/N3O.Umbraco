using HandlebarsDotNet;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars;

public class HandlebarsArguments {
    private readonly IJsonProvider _jsonProvider;
    private readonly Arguments _arguments;

    public HandlebarsArguments(IJsonProvider jsonProvider, Arguments arguments) {
        _jsonProvider = jsonProvider;
        _arguments = arguments;
    }
    
    public TValue Get<TValue>(int index) {
        var argument = _arguments[index];

        if (argument == default) {
            return default;
        }
        
        if (argument is TValue value) {
            return value;
        }

        var json = _jsonProvider.SerializeObject(argument);

        return _jsonProvider.DeserializeObject<TValue>(json);
    }

    public bool TryGet<TValue>(int index, out TValue value) {
        try {
            value = Get<TValue>(index);
            
            return true;
        } catch {
            value = default;
            
            return false;
        }
    }
}
