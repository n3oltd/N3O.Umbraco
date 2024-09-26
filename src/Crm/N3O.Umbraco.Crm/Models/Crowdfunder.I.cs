using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crm.Models;

public interface ICrowdfunder {
    Guid Id { get; }
    string Name { get; }
    Currency Currency { get; }
    bool Activate { get; }
    bool Deactivate { get; }
    IEnumerable<ICrowdfunderGoal> Goals { get; }

    string Url(IContentLocator contentLocator);
}