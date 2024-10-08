using N3O.Umbraco.Crm.Lookups;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfunderData {
    Guid Id { get; }
    CrowdfunderType Type { get; }
    string Name { get; }
    string Url { get; }
    string Comment { get; }
    bool Anonymous { get; }
}