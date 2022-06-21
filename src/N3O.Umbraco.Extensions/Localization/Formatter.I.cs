namespace N3O.Umbraco.Localization;

public interface IFormatter {
    INumberFormatter Number { get; }
    IDateTimeFormatter DateTime { get; }
    ITextFormatter Text { get; }
}
