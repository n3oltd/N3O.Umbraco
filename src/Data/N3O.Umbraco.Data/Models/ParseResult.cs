using Newtonsoft.Json;

namespace N3O.Umbraco.Data.Models;

public interface IParseResult {
    public bool Success { get; }
    public object Value { get; }
}

public class ParseResult<T> : IParseResult {
    public ParseResult(bool success, T value) {
        Success = success;
        Value = value;
    }

    public bool Success { get; }
    public T Value { get; }

    [JsonIgnore]
    object IParseResult.Value => Value;
}

public static class ParseResult {
    public static ParseResult<T> Success<T>(T value) {
        return new ParseResult<T>(true, value);
    }

    public static ParseResult<T> Fail<T>() {
        return new ParseResult<T>(false, default);
    }
}
