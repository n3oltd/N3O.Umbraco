using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public interface ITextConverter<in TValue> {
    string Convert(IFormatter formatter, TValue value);
}
