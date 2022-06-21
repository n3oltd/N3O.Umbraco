using Newtonsoft.Json;

namespace N3O.Umbraco;

public interface IRange {
    object From { get; }
    object To { get; }
}

public class Range<T> : IRange {
    [JsonConstructor]
    public Range() { }

    public Range(T from, T to) {
        From = from;
        To = to;
    }

    public T From { get; set; }
    public T To { get; set; }

    [JsonIgnore]
    object IRange.From => From;

    [JsonIgnore]
    object IRange.To => To;
}
