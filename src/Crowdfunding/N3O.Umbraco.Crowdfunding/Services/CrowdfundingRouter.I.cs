using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding;

public interface ICrowdfundingRouter {
    ICrowdfundingPage CurrentPage { get; }
    Uri RequestUri { get; }
    IReadOnlyDictionary<string, string> RequestQuery { get; }
}