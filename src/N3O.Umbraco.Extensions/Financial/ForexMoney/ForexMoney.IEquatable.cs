using System;

namespace N3O.Umbraco.Financial;

public partial class ForexMoney : IEquatable<ForexMoney> {
    public bool Equals(ForexMoney other) {
        if (other == null) {
            return false;
        }

        if (Base.Currency != other.Base.Currency) {
            return false;
        }

        if (Base.Amount != other.Base.Amount) {
            return false;
        }

        if (Quote.Currency != other.Quote.Currency) {
            return false;
        }

        if (Quote.Amount != other.Quote.Amount) {
            return false;
        }

        if (ExchangeRate != other.ExchangeRate) {
            return false;
        }

        return true;
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((ForexMoney) obj);
    }
}
