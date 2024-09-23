using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class GoalOptionRes {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public AllocationType Type { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public GoalOptionFundDimensionRes Dimension1 { get; set; }
    public GoalOptionFundDimensionRes Dimension2 { get; set; }
    public GoalOptionFundDimensionRes Dimension3 { get; set; }
    public GoalOptionFundDimensionRes Dimension4 { get; set; }
    public DonationItemRes Fund { get; set; }
    public FeedbackSchemeRes Feedback { get; set; }
}