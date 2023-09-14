using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class CheckoutCompletedReq {
    [Name("ID")]
    [FromQuery(Name = "id")]
    public string Id { get; set; }
    
    [Name("Resource Path")]
    [FromQuery(Name = "resourcePath")]
    public string ResourcePath { get; set; }
}
