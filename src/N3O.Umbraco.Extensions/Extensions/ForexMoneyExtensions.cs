using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions {
    public static class ForexMoneyExtensions {
        public static bool IsZero(this ForexMoney money) {
            return money.Base.Amount == 0 && money.Quote.Amount == 0;
        }
    
        public static ForexMoney Sum(this IEnumerable<ForexMoney> collection) {
            var list = collection.OrEmpty().ToList();

            if (list.None()) {
                throw new Exception($"{nameof(collection)} must contain at least one element");
            }

            var first = list.First();
            var rest = list.Skip(1).ToList();

            if (rest.None()) {
                return first;
            }

            var sum = first.Add(rest);

            return sum;
        }
    
        public static ForexMoney Zero(this ForexMoney money) {
            return new ForexMoney(money.Base.Zero(), money.Quote.Zero());
        }
    }
}
