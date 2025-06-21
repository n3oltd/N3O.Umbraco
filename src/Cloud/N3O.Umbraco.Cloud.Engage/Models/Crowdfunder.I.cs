using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Engage.Models;

public interface ICrowdfunder {
    Guid Id { get; }
    CrowdfunderType Type { get; }
    string Name { get; }
    Currency Currency { get; }
    IEnumerable<ICrowdfunderGoal> Goals { get; }
    CrowdfunderStatus Status { get; }
    
    string Url(IServiceProvider serviceProvider);
}