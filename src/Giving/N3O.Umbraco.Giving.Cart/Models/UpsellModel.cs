using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Giving.Cart.Models;

public class UpsellModel {
    public UpsellModel(Guid id, string title, string description, Money amount) {
        Id = id;
        Title = title;
        Description = description;
        Amount = amount;
    }

    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }
    public Money Amount { get; }
}