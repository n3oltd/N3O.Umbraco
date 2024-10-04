using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crm.Models;

public interface ICrowdfunder {
    Guid Id { get; }
    string Name { get; }
    Currency Currency { get; }
    IEnumerable<ICrowdfunderGoal> Goals { get; }
    CrowdfunderStatus Status { get; }
    
    string Url(IServiceProvider serviceProvider);
}