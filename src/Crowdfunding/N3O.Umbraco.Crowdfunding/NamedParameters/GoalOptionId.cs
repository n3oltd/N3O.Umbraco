using N3O.Umbraco.Parameters;
using System;

namespace N3O.Umbraco.Crowdfunding.NamedParameters; 

public class GoalOptionId : NamedParameter<string> {
    public override string Name => "goalOptionId";
}