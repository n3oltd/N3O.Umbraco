﻿using CsvHelper.Configuration.Attributes;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeebackCrowdfundingCartItemReq {
    [Name("Custom Fields")]
    public FeedbackNewCustomFieldsReq CustomFields { get; set; }
}