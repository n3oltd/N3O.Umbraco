using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingDataReq {
    [Name("Fundraiser ID")]
    public Guid? FundraiserId { get; set; }

    [Name("Comment")]
    public string Comment { get; set; }

    [Name("Anonymous")]
    public bool Anonymous { get; set; }
}