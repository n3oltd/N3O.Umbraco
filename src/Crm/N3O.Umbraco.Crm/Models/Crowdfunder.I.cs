using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crm.Models;

public interface ICrowdfunder {
    Guid Id { get; }
    string Name { get; }
    string Url { get; }
    Currency Currency { get; }
    IEnumerable<ICrowdfunderGoal> Goals { get; }
}