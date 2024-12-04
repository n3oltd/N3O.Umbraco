using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class UpsellOffer {
    public UpsellOffer(bool allowMultiple,
                       Guid upsellOfferId,
                       string title,
                       string description,
                       IEnumerable<GivingType> offeredFor,
                       GivingType givingType,
                       Money price,
                       IEnumerable<PriceHandleElement> priceHandles) {
        AllowMultiple = allowMultiple;
        UpsellOfferId = upsellOfferId;
        Title = title;
        Description = description;
        GivingType = givingType;
        OfferedFor = offeredFor;
        Price = price;
        PriceHandles = priceHandles;
    }

    public bool AllowMultiple { get; }
    public Guid UpsellOfferId { get; }
    public string Title { get; }
    public string Description { get; }
    public GivingType GivingType { get; }
    public IEnumerable<GivingType> OfferedFor { get; }
    public Money Price { get; }
    public IEnumerable<PriceHandleElement> PriceHandles { get; }
}