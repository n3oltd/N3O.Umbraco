using Humanizer;
using N3O.Umbraco.Financial;
using System.Globalization;

namespace N3O.Umbraco.Localization {
    public class NumberFormatter : INumberFormatter {
        private readonly ILocalizationSettingsAccessor _settingsAccessor;

        public NumberFormatter(ILocalizationSettingsAccessor settingsAccessor) {
            _settingsAccessor = settingsAccessor;
        }

        public string FormatOrdinal(int number, NumberFormat numberFormat = null) {
            if (numberFormat == null) {
                numberFormat = _settingsAccessor.GetSettings().NumberFormat;
            }

            var cultureInfo = new CultureInfo(numberFormat.CultureCode);

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
                numberFormat = _settingsAccessor.GetSettings().NumberFormat;
            }

            var numberFormatInfo = GetNumberFormatInfo(numberFormat.CultureCode);

            return numberFormatInfo;
        }

        private NumberFormatInfo GetNumberFormatInfo(string cultureCode) {
            return ((CultureInfo)CultureInfo.GetCultureInfo(cultureCode).Clone()).NumberFormat;
        }
    
        public static INumberFormatter Invariant = new NumberFormatter(DefaultLocalizationSettingsAccessor.Instance);
    }
}
