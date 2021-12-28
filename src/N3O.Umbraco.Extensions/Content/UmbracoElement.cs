using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Element;

public abstract class UmbracoElement : Value, IUmbracoElement {
    public PublishedElementModel Content { get; set; }
    
    protected TProperty GetAs<TContent, TProperty>(Expression<Func<TContent, TProperty>> memberExpression) {
        var alias = AliasHelper.ForProperty(memberExpression);
        var value = (PublishedContentModel) Content.Value(alias);

        return value.As<TProperty>();
    }
    
    protected IEnumerable<TProperty> GetCollectionAs<TContent, TProperty>(Expression<Func<TContent, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper.ForProperty(memberExpression);
        var values = (IEnumerable) Content.Value(alias);

        return values.Cast<PublishedContentModel>().Select(x => x.As<TProperty>());
    }
    
    protected TProperty GetValue<TContent, TProperty>(Expression<Func<TContent, TProperty>> memberExpression) {
        var alias = AliasHelper.ForProperty(memberExpression);

        return Content.Value<TProperty>(alias);
    }
}