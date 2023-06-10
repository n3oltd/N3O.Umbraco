namespace N3O.Umbraco.Localization; 

public interface IFormatterFactory {
    IFormatter CreateFormatter(LocalizationSettings localizationSettings = null);
}