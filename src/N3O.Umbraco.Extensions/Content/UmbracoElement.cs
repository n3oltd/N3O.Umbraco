using N3O.Umbraco.Extensions;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Content;

public abstract class UmbracoElement<T> : Value, IUmbracoElement {
    private IPublishedElement _content;
    private IPublishedContent _parent;

    // Do not use get/set property as causes issues with model validation
    public virtual IPublishedElement Content() => _content;
    public virtual IPublishedContent Parent() => _parent;

    public virtual void Content(IPublishedElement content, IPublishedContent parent) {
        _content = content;
        _parent = parent;
    }

    protected TProperty GetAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (IPublishedContent) Content().Value(alias);

        return value.As<TProperty>();
    }

    protected IEnumerable<TProperty> GetCollectionAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias);

        return values.Cast<IPublishedContent>().Select(x => x.As<TProperty>());
    }
    
    protected IEnumerable<TProperty> GetNestedAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias) ?? Enumerable.Empty<IPublishedElement>();

        return values.Cast<IPublishedElement>().Select(x => x.As<TProperty>(_parent));
    }
    
    protected TProperty GetPickedAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = Content().Value(alias);

        if (value is TProperty typedValue) {
            return typedValue;
        } else {
            return ((IPublishedContent) value).As<TProperty>();
        }
    }
    
    protected IEnumerable<TProperty> GetPickedAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias) ?? Enumerable.Empty<IPublishedContent>();

        return values.Cast<IPublishedContent>().Select(x => x.As<TProperty>());
    }

    protected LocalDate? GetLocalDate(Expression<Func<T, LocalDate?>> memberExpression) {
        return GetConvertedValue<DateTime?, LocalDate?>(memberExpression, dt => dt?.ToLocalDate());
    }
    
    protected LocalDate GetLocalDate(Expression<Func<T, LocalDate>> memberExpression) {
        return GetConvertedValue<DateTime, LocalDate>(memberExpression, dt => dt.ToLocalDate());
    }
    
    protected TProperty GetValue<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);

        var property = Content().GetProperty(alias);

        if (property == null) {
            return default;
        }

        var propertyValue = property.GetValue();

        if (propertyValue is TProperty typedProperty) {
            return typedProperty;
        } else if (propertyValue is IPublishedContent publishedContent) {
            return publishedContent.As<TProperty>();
        } else if (propertyValue is IPublishedElement publishedElement) {
            return publishedElement.As<TProperty>(_parent);
        } else {
            return default;
        }
    }
    
    protected TConverted GetConvertedValue<TProperty, TConverted>(Expression<Func<T, TConverted>> memberExpression,
                                                                  Func<TProperty, TConverted> convert) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);

        return convert(Content().Value<TProperty>(alias));
    }
}
