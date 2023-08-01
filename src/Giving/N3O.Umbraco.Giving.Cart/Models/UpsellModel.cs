using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Content;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Cart.Models;

public class UpsellModel {
    public UpsellModel(Guid id, string title, string description, Money amount, IEnumerable<PriceHandleElement> priceHandles) {
        Id = id;
        Title = title;
        Description = description;
        Amount = amount;
        PriceHandles = priceHandles;
    }

    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }
    public Money Amount { get; }
    public IEnumerable<PriceHandleElement> PriceHandles { get; }
}