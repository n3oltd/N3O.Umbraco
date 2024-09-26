using CsvHelper.Configuration.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using System;

namespace N3O.Umbraco.Crowdfunding.Models.AddToCart;

public class AddToCartItemReq {
    [Name("Goal ID")]
    public Guid? GoalId { get; set; }

    [Name("Value")]
    public MoneyReq Value { get; set; }

    [Name("Custom Fields")]
    public FeedbackNewCustomFieldsReq CustomFields { get; set; }
}