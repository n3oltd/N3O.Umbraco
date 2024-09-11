﻿using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class AvailableGoalRes {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public AllocationType Type { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public DonationItemRes Fund { get; set; }
    public FeedbackSchemeRes Feedback { get; set; }
}