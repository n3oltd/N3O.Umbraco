using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Financial {
    public partial class ForexMoney {
        public static bool operator ==(ForexMoney lhs, ForexMoney rhs) {
            if (ReferenceEquals(lhs, rhs)) {
                return true;
            }

            if (ReferenceEquals(lhs, null)) {
                return false;
            }

            if (ReferenceEquals(rhs, null)) {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ForexMoney lhs, ForexMoney rhs) {
            return !(lhs == rhs);
        }

        public ForexMoney Add(ForexMoney f2) {
            return Add(f2.Yield());
        }

        public ForexMoney Add(IEnumerable<ForexMoney> forexMonies) {
            return Add(forexMonies.ToArray());
        }

        public ForexMoney Add(params ForexMoney[] forexMonies) {
            var list = forexMonies.OrEmpty().ToList();

            if (list.None()) {
                throw new Exception($"{nameof(forexMonies)} must contain at least one element");
            }

            if (!list.Select(x => x.Base.Currency).Distinct().IsSingle()) {
                throw new Exception($"{nameof(forexMonies)} must all have the same base currency");
            }

            if (!list.Select(x => x.Quote.Currency).Distinct().IsSingle()) {
                throw new Exception($"{nameof(forexMonies)} must all have the same quote currency");
            }

            var baseTotal = list.Sum(x => x.Base.Amount);
            baseTotal += Base.Amount;
            var baseCurrency = list.First().Base.Currency;

            var baseMoney = new Money(baseTotal, baseCurrency);

            var quoteTotal = list.Sum(x => x.Quote.Amount);
            quoteTotal += Quote.Amount;
            var quoteCurrency = list.First().Quote.Currency;

            var quoteMoney = new Money(quoteTotal, quoteCurrency);

            var forex = new ForexMoney(baseMoney, quoteMoney);

            return forex;
        }

        public static ForexMoney operator +(ForexMoney f1, ForexMoney f2) {
            if (f1 == null || f2 == null) {
                return null;
            }

            return f1.Add(f2);
        }

        public static ForexMoney operator -(ForexMoney f1, ForexMoney f2) {
            if (f1.Base.Currency != f2.Base.Currency) {
                throw new Exception($"{nameof(f1)} and {nameof(f2)} do not have the same base currency");
            }

            if (f1.Quote.Currency != f2.Quote.Currency) {
                throw new Exception($"{nameof(f1)} and {nameof(f2)} do not have the same quote currency");
            }

            var baseCurrency = f1.Base.Currency;
            var baseAmount = f1.Base.Amount - f2.Base.Amount;
            var baseMoney = new Money(baseAmount, baseCurrency);

            var quoteCurrency = f1.Quote.Currency;
            var quoteAmount = f1.Quote.Amount - f2.Quote.Amount;
            var quoteMoney = new Money(quoteAmount, quoteCurrency);

            var totalMoney = new ForexMoney(baseMoney, quoteMoney);

            return totalMoney;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Base.GetHashCode(), Quote.GetHashCode(), ExchangeRate.GetHashCode());
        }
    }
}