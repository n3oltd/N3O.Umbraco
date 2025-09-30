using N3O.Umbraco.Attributes;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Scheduler.Models;

public class ProxyReq {
    [Name("Command Type")]
    public Type CommandType { get; set; }
    
    [Name("Model Type")]
    public Type RequestType { get; set; }
    
    [Name("Request Body")]
    public string RequestBody { get; set; }
    
    [Name("Parameter Data")]
    public Dictionary<string, string> ParameterData { get; set; }
}