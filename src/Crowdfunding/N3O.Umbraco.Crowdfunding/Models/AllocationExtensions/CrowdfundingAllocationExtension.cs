using FluentValidation;
using FluentValidation.Results;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

// We would use a composer to scan assemblies for all IAllocationValidator implementations and we would register
// these as the interface. We can then inject an IEnumerable<IAllocationValidator> into the AllocationReqValidator
// constructor, and we can then loop through and call validate on these to ensure the validation all happens before
// the request proceeds.

public interface IAllocationExtensionValidator {
    ValidationResult Validate(AllocationReq allocationReq);
}

public interface IAllocationExtensionBinder {
    object Bind(AllocationReq allocationReq);
}

public class CrowdfundingAllocationExtension : AllocationExtensionValidator<CrowdfundingDataReq> {
    public CrowdfundingAllocationExtension(IJsonProvider jsonProvider, IValidator<CrowdfundingDataReq> validator)
        : base(jsonProvider, validator) { }

    protected override string Key => "crowdfunding";
}

public abstract class AllocationExtensionValidator<TReq> : IAllocationExtensionValidator {
    private readonly IJsonProvider _jsonProvider;
    private readonly IValidator<TReq> _validator;

    protected AllocationExtensionValidator(IJsonProvider jsonProvider, IValidator<TReq> validator) {
        _jsonProvider = jsonProvider;
        _validator = validator;
    }
    
    public ValidationResult Validate(AllocationReq allocationReq) {
        if (allocationReq.Extensions.HasValue() && allocationReq.Extensions.ContainsKey(Key)) {
            // TODO Fix this line, we want to convert the JObject to TReq, lots of places in Karakoram code where we do
            // this just can't remember how
            var extensionDataReq = _jsonProvider.DeserializeObject<TReq>(allocationReq.Extensions[Key].ToString());

            return _validator.Validate(extensionDataReq);
        } else {
            return new ValidationResult();   
        }
    }
    
    protected abstract string Key { get; }
}

public class CrowdfundingAllocationExtensionBinder : AllocationExtensionBinder<CrowdfundingDataReq, CrowdfundingData> {
    public CrowdfundingAllocationExtensionBinder(IJsonProvider jsonProvider) : base(jsonProvider) { }
    
    protected override CrowdfundingData Bind(CrowdfundingDataReq req) {
        return new CrowdfundingData(req);
    }
    
    protected override string Key => "crowdfunding";
}

public abstract class AllocationExtensionBinder<TReq, TModel> : IAllocationExtensionBinder {
    private readonly IJsonProvider _jsonProvider;

    protected AllocationExtensionBinder(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }
    
    public object Bind(AllocationReq allocationReq) {
        // Notice how similar pattern is here to above, hence we may want to use extension method or some other way
        // to avoid code duplication
        if (allocationReq.Extensions.HasValue() && allocationReq.Extensions.ContainsKey(Key)) {
            // TODO Fix this line, we want to convert the JObject to TReq, lots of places in Karakoram code where we do
            // this just can't remember how
            var extensionDataReq = _jsonProvider.DeserializeObject<TReq>(allocationReq.Extensions[Key].ToString());

            return Bind(extensionDataReq);
        } else {
            return null;   
        }
    }

    protected abstract TModel Bind(TReq req);
    
    protected abstract string Key { get; }
}

// TODO When we are passing the allocationReq into the allocation model constructor, before we do this we should
// inject the IEnumerable<IAllocationExtensionBinder> and use this to run all the custom binders to get the strongly
// types model for allocation extension data.

// THe pattern for these is very similar to what we already have for block models and extension data in the N3O.Umbraco.Extensions
// project, e.g. see search for an example
public static class CrowdfundingAllocationExtensions {
    public static CrowdfundingData GetCrowdfundingData(this IAllocation allocation) {
        // Check allocation.Extensions for our key and retrieve it if exists
        throw new NotImplementedException();
    }
    
    public static Allocation SetCrowdfundingData(this IAllocation allocation, CrowdfundingData extensionData) {
        // Add or replace the extension data under the relevant key/
        throw new NotImplementedException();
    }

    public static bool HasCrowdfundingData(this IAllocation allocation) {
        throw new NotImplementedException();
    }
}

/*
 * E.g. in the cart view we can now do if (allocation.HasCrowdfundingData()) {
 *      fetch the data and use it to show the comment, page url or whatever else
 * }
*/