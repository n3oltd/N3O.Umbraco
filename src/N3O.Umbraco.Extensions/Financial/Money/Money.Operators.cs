using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Financial {
    public partial class Money {
        public static bool operator ==(Money lhs, Money rhs) {
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

        public static bool operator !=(Money lhs, Money rhs) {
            return !(lhs == rhs);
        }

        public Money Add(Money rhs) {
            return Add(rhs.Yield());
        }

        public Money Add(IEnumerable<Money> monies) {
            return Add(monies.ToArray());
        }

        public Money Add(params Money[] monies) {
            var list = monies.OrEmpty().ToList();

            if (list.None()) {
                throw new Exception($"{nameof(monies)} must contain at least one element");
            }

            if (list.Any(x => x.Currency != Currency)) {
                throw new Exception($"{nameof(monies)} must all have the same currency");
            }

            var total = Amount + list.Sum(x => x.Amount);
            var money = new Money(total, Currency);

            return money;
        }

        public static Money operator +(Money lhs, Money rhs) {
            if (lhs == null || rhs == null) {
                return null;
            }

            return lhs.Add(rhs);
        }

        public static Money operator -(Money lhs, Money rhs) {
            if (lhs.Currency != rhs.Currency) {
                throw new Exception($"{nameof(lhs)} and {nameof(rhs)} do not have the same currency");
            }
        
            var currency = lhs.Currency;
            var amount = lhs.Amount - rhs.Amount;
            var money = new Money(amount, currency);

            return money;
        }
        
        public static Money operator *(Money lhs, decimal rhs) {
            if (lhs == null) {
                return null;
            }

            return new Money(lhs.Amount * rhs, lhs.Currency);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Currency.GetHashCode(), Amount.GetHashCode());
        }
    }
}