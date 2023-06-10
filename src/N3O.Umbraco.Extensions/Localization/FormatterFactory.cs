namespace N3O.Umbraco.Localization; 

public class FormatterFactory : IFormatterFactory {
    private readonly IStringLocalizer _stringLocalizer;

    public FormatterFactory(IStringLocalizer stringLocalizer) {
        _stringLocalizer = stringLocalizer;
    }
    
    public IFormatter CreateFormatter(LocalizationSettings localizationSettings = null) {
        return new Formatter(NumberFormatter.Create(localizationSettings),
                             DateTimeFormatter.Create(localizationSettings),
                             TextFormatter.Create(_stringLocalizer));
    }
}