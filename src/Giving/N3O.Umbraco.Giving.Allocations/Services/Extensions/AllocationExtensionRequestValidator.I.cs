using FluentValidation.Results;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations;

public interface IAllocationExtensionRequestValidator {
    ValidationResult Validate(IDictionary<string, JToken> req);
    
    string Key { get; }
}