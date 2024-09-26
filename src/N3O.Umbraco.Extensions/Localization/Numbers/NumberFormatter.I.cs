using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Localization;

public interface INumberFormatter {
    string FormatOrdinal(int number, NumberFormat numberFormat = null);
    string Format(int? amount, NumberFormat numberFormat = null);
    string Format(long? amount, NumberFormat numberFormat = null);
    string Format(decimal? amount, NumberFormat numberFormat = null);
    string FormatMoneyAbbreviated(Money money, NumberFormat numberFormat = null);
    string FormatMoney(Money money, NumberFormat numberFormat = null);
    string FormatMoney(decimal amount, Currency currency, NumberFormat numberFormat = null);
    string FormatPercentage(decimal number, int decimalDigits);
    
    NumberFormat NumberFormat { get; }
}
