using FluentValidation.Results;
using J2N.Collections.Generic;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data;

public abstract class ContentPropertyValidator<TReq, TRes> : IContentPropertyValidator 
    where TReq : ValueReq
    where TRes : ContentPropertyConfigurationRes {
    private readonly List<ValidationFailure> Failures = [];
    
    private readonly IFormatter _formatter;
    private readonly string _contentTypeAlias;
    private readonly string _propertyAlias;

    protected ContentPropertyValidator(IFormatter formatter, string contentTypeAlias, string propertyAlias) {
        _formatter = formatter;
        _contentTypeAlias = contentTypeAlias;
        _propertyAlias = propertyAlias;
    }
    
    public bool IsValidator(string contentTypeAlias, string propertyAlias) {
        return contentTypeAlias.EqualsInvariant(_contentTypeAlias) && propertyAlias.EqualsInvariant(_propertyAlias);
    }
    
    public void PopulatePropertyConfiguration(IPropertyType property, ContentPropertyConfigurationRes res) {
        PopulatePropertyConfiguration(property, (TRes) res);
    }

    public ValidationResult Validate(IPublishedContent content, string propertyAlias, ValueReq req) {
        Validate(content, propertyAlias, (TReq) req);

        var result = GetValidationResult();

        return result;
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

    private ValidationResult GetValidationResult() {
        if (Failures.HasAny()) {
            return new ValidationResult(Failures);
        }

        return new ValidationResult();
    }

    protected abstract void PopulatePropertyConfiguration(IPropertyType property, TRes res);
    protected abstract void Validate(IPublishedContent content, string propertyAlias, TReq req);
}