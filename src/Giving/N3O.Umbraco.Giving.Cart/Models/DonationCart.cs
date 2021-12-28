using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using NodaTime;
using System;

namespace N3O.Umbraco.Giving.Cart.Models;

public class DonationCart : Value {
    public DonationCart(Guid id, Instant timestamp, Currency currency, CartContents single, CartContents regular) {
        Id = id;
        Timestamp = timestamp;
        Currency = currency;
        Single = single;
        Regular = regular;
    }

    public Guid Id { get; }
    public Instant Timestamp { get; }
    public Currency Currency { get; }
    public CartContents Single { get; }
    public CartContents Regular { get; }

    public bool IsEmpty() => Single.IsEmpty() && Regular.IsEmpty();

    public static DonationCart Create(Instant now, Currency currency) {
        return new DonationCart(Guid.NewGuid(),
                                now,
                                currency,
                                CartContents.Create(currency, DonationTypes.Single),
                                CartContents.Create(currency, DonationTypes.Regular));
    }
}