using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Content;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class UpsellOffer {
    public UpsellOffer(Guid upsellId,
                       string title,
                       string description,
                       Money price,
                       IEnumerable<PriceHandleElement> priceHandles) {
        UpsellId = upsellId;
        Title = title;
        Description = description;
        Price = price;
        PriceHandles = priceHandles;
    }

    public Guid UpsellId { get; }
    public string Title { get; }
    public string Description { get; }
    public Money Price { get; }
    public IEnumerable<PriceHandleElement> PriceHandles { get; }
}