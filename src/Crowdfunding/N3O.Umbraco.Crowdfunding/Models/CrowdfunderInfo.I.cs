using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.References;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfunderInfo {
    Guid Id { get; }
    Reference Reference { get; }
    CrowdfunderType Type { get; }
    IFundraiserInfo Fundraiser { get; }
}