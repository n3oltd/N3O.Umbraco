using Humanizer;
using N3O.Umbraco.Financial;
using System.Globalization;

namespace N3O.Umbraco.Localization;

public class NumberFormatter : INumberFormatter {
    public NumberFormatter(ILocalizationSettingsAccessor settingsAccessor) : this(settingsAccessor.GetSettings()) { }

    private NumberFormatter(LocalizationSettings settings) {
        NumberFormat = settings.NumberFormat;
    }

    public string FormatOrdinal(int number, NumberFormat numberFormat = null) {
        if (numberFormat == null) {
            numberFormat = NumberFormat;
        }

        var cultureInfo = numberFormat.GetCultureInfo();

        var ordinal = number.Ordinalize(cultureInfo);

        return ordinal;
    }

    public string Format(int? amount, NumberFormat numberFormat = null) {
        return Format(amount, 0, numberFormat);
    }

    public string Format(long? amount, NumberFormat numberFormat = null) {
        return Format(amount, 0, numberFormat);
    }

    public string Format(decimal? amount, NumberFormat numberFormat = null) {
        return Format(amount, 2, numberFormat);
    }

    public string FormatMoney(Money money, NumberFormat numberFormat = null) {
        if (money == null) {
            return null;
        }

        return FormatMoney(money, numberFormat, false);
    }

    public string FormatMoney(decimal amount, Currency currency, NumberFormat numberFormat = null) {
        return FormatMoney(new Money(amount, currency), numberFormat);
    }

    public NumberFormat NumberFormat { get; }

    public string FormatMoneyAbbreviated(Money money, NumberFormat numberFormat = null) {
        if (money == null) {
            return null;
        }
    
        return FormatMoney(money, numberFormat, true);
    }

    private string Format(decimal? amount, int decimalDigits, NumberFormat numberFormat = null) {
        if (amount == null) {
            return null;
        }

        var localFormat = GetNumberFormatInfo(numberFormat);

        return amount.Value.ToString($"N{decimalDigits}", localFormat);
    }

    private string FormatMoney(Money money, NumberFormat numberFormat, bool abbreviated) {
        var localFormat = GetNumberFormatInfo(numberFormat);

        localFormat.CurrencySymbol = money.Currency.Symbol;

        var formatProvider = abbreviated ? AbbreviatedNumberFormatter.Provider(localFormat) : localFormat;

        return string.Format(formatProvider, money.Amount % 1m == 0m ? "{0:C0}" : "{0:C}", money.Amount);
    }

    private NumberFormatInfo GetNumberFormatInfo(NumberFormat numberFormat) {
        if (numberFormat == null) {
            numberFormat = NumberFormat;
        }

        var numberFormatInfo = numberFormat.GetNumberFormatInfo();

        return numberFormatInfo;
    }

    public static readonly INumberFormatter Default =
        new NumberFormatter(DefaultLocalizationSettingsAccessor.Instance);

    public static INumberFormatter Create(LocalizationSettings settings) {
        return new NumberFormatter(settings);
    }
}
