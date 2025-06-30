using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.References;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfunderInfo {
    Guid Id { get; }
    Reference Reference { get; }
    CrowdfunderType Type { get; }
    CrowdfunderStatus Status { get; }
    IFundraiserInfo Fundraiser { get; }
}