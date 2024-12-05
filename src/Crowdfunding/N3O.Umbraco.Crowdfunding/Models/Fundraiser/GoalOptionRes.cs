
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class GoalOptionRes {
    public string Id { get; set; }
    public string Name { get; set; }
    public AllocationType Type { get; set; }
    public IEnumerable<TagRes> Tags { get; set; }
    public GoalOptionFundDimensionRes Dimension1 { get; set; }
    public GoalOptionFundDimensionRes Dimension2 { get; set; }
    public GoalOptionFundDimensionRes Dimension3 { get; set; }
    public GoalOptionFundDimensionRes Dimension4 { get; set; }
    public DonationItemRes Fund { get; set; }
    public FeedbackSchemeRes Feedback { get; set; }
}