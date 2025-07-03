namespace N3O.Umbraco.Localization;

public class Formatter : IFormatter {
    public Formatter(INumberFormatter numberFormatter,
                     IDateTimeFormatter dateTimeFormatter,
                     ITextFormatter textFormatter) {
        Number = numberFormatter;
        DateTime = dateTimeFormatter;
        Text = textFormatter;
    }

    public INumberFormatter Number { get; }
    public IDateTimeFormatter DateTime { get; }
    public ITextFormatter Text { get; }

    public static IFormatter Default => new Formatter(NumberFormatter.Default,
                                                      DateTimeFormatter.Default,
                                                      TextFormatter.Default);
}
