using System;

namespace N3O.Umbraco.Extensions;

public static class DecimalExtensions {
    public static bool HasValue(this decimal? value) {
        if (value == null) {
            return false;
        }

        return true;
    }

    public static decimal? RoundMoney(this decimal? amount) {
        if (amount == null) {
            return null;
        } else {
            return Math.Round(amount.Value, 2, MidpointRounding.AwayFromZero);
        }
    }
    
    public static decimal RoundMoney(this decimal amount) {
        return Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }
}
