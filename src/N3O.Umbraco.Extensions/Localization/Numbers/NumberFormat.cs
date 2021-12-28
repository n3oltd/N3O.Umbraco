using N3O.Umbraco.Constants;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Localization {
    public class NumberFormat : NamedLookup {
        public NumberFormat(string id,
                            string name,
                            string cultureCode,
                            string decimalSeparator,
                            string thousandsSeparator)
            : base(id, name) {
            CultureCode = cultureCode;
            DecimalSeparator = decimalSeparator;
            ThousandsSeparator = thousandsSeparator;
        }

        public string CultureCode { get; }
        public string DecimalSeparator { get; }
        public string ThousandsSeparator { get; }
    }

    public class NumberFormats : StaticLookupsCollection<NumberFormat> {
        public static readonly NumberFormat International = new NumberFormat("international", "UK/US Style", "en-GB", DecimalSeparators.Point, ThousandsSeparators.Comma);
        public static readonly NumberFormat EU1 = new NumberFormat("eu1", "European Style 1", "fr-FR", DecimalSeparators.Comma, ThousandsSeparators.Space);
        public static readonly NumberFormat EU2 = new NumberFormat("eu2", "European Style 2", "es-ES", DecimalSeparators.Comma, ThousandsSeparators.Point);
    }
}