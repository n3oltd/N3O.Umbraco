using System;

namespace N3O.Umbraco.Financial;

public partial class Money : IEquatable<Money> {
    public bool Equals(Money other) {
        if (other == null) {
            return false;
        }

        if (Currency != other.Currency) {
            return false;
        }

        if (Amount != other.Amount) {
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

        return obj.GetType() == GetType() && Equals((Money)obj);
    }
}
