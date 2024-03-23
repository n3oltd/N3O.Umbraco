using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Content;

public abstract class UmbracoElement<T> : Value, IUmbracoElement {
    public IPublishedElement Content { get; set; }

    protected TProperty GetAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (IPublishedContent) Content.Value(alias);

        return value.As<TProperty>();
    }

    protected IEnumerable<TProperty> GetCollectionAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content.Value(alias);
        
        if (!values.HasValue()) {
            return default;
        }

        if (values is IEnumerable<TProperty> typedProperty) {
            return typedProperty;
        } else if (values is IEnumerable<IPublishedContent> publishedContent) {
            return publishedContent.Select(x => x.As<TProperty>());
        } else if (values is IEnumerable<IPublishedElement> publishedElement) {
            return publishedElement.Select(x => x.As<TProperty>());
        } else {
            return default;
        }
    }

    protected TProperty GetValue<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);

        var property = Content.GetProperty(alias);

        if (property == null) {
            return default;
        }

        var propertyValue = property.GetValue();

        if (propertyValue is TProperty typedProperty) {
            return typedProperty;
        } else if (propertyValue is IPublishedContent publishedContent) {
            return publishedContent.As<TProperty>();
        } else if (propertyValue is IPublishedElement publishedElement) {
            return publishedElement.As<TProperty>();
        } else {
            return default;
        }
    }
}
