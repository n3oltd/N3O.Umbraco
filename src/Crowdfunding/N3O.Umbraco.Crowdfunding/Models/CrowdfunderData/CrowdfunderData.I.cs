using N3O.Umbraco.Crowdfunding.Lookups;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfunderData {
    Guid CrowdfunderId { get; }
    CrowdfunderType CrowdfunderType { get; }
    string Comment { get; }
    bool Anonymous { get; }
}