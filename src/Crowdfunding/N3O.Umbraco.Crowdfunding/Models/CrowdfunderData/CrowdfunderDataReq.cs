using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderDataReq {
    [Name("Crowdfunder ID")]
    public Guid? CrowdfunderId { get; set; }

    [Name("Comment")]
    public string Comment { get; set; }

    [Name("Anonymous")]
    public bool? Anonymous { get; set; }
}