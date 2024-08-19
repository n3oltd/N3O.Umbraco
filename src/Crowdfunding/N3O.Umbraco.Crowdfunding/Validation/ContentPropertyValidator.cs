using FluentValidation.Results;
using J2N.Collections.Generic;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public abstract class ContentPropertyValidator<T> : IContentPropertyValidator where T : ValueReq {
    protected readonly List<ValidationFailure> Failures = new();
    
    private readonly IFormatter _formatter;
    private readonly string _contentTypeAlias;
    private readonly string _propertyAlias;

    protected ContentPropertyValidator(IFormatter formatter, string contentTypeAlias, string propertyAlias) {
        _formatter = formatter;
        _contentTypeAlias = contentTypeAlias;
        _propertyAlias = propertyAlias;
    }
    
    protected void AddFailure<TStrings>(string propertyAlias,
                                        Func<TStrings, string> propertySelector,
                                        params object[] formatArgs) 
        where TStrings : class, IStrings, new() {
        AddFailure(propertyAlias, f => f.Text.Format(propertySelector, formatArgs));
    }
    
    private void AddFailure(string propertyAlias, Func<IFormatter, string> getErrorMessage) {
        Failures.Add(new ValidationFailure(propertyAlias, getErrorMessage(_formatter)));
    }
    
    public bool IsValidator(string contentTypeAlias, string propertyAlias) {
        return contentTypeAlias.EqualsInvariant(_contentTypeAlias) && propertyAlias.EqualsInvariant(_propertyAlias);
    }

    public ValidationResult Validate(IPublishedContent content, string propertyAlias, ValueReq req) {
        Validate(content, propertyAlias, (T) req);

        var result = GetValidationResult();

        return result;
    }

    private ValidationResult GetValidationResult() {
        if (Failures.HasAny()) {
            return new ValidationResult(Failures);
        }

        return new ValidationResult();
    }

    protected abstract void Validate(IPublishedContent content, string propertyAlias, T req);
}