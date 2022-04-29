using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models {
    public class ParserSettings : Value {
        public ParserSettings(DatePattern datePattern, DecimalSeparator decimalSeparator, string storageFolderName) {
            DatePattern = datePattern;
            DecimalSeparator = decimalSeparator;
            StorageFolderName = storageFolderName;
        }

        public DatePattern DatePattern { get; }
        public DecimalSeparator DecimalSeparator { get; }
        public string StorageFolderName { get; }
    }
}