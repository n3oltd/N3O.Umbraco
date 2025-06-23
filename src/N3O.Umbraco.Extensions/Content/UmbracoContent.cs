using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Content;

public abstract class UmbracoContent<T> : Value, IUmbracoContent {
    private IPublishedContent _content;

    // Do not use get/set property as causes issues with model validation
    public virtual IPublishedContent Content() => _content;
    public virtual void Content(IPublishedContent content) => _content = content;

    protected TProperty Child<TProperty>(Expression<Func<T, TProperty>> memberExpression)
        where TProperty : UmbracoContent<TProperty> {
        var alias = AliasHelper<TProperty>.ContentTypeAlias();
        var child = Content().Children.SingleOrDefault(x => x.ContentType.Alias.EqualsInvariant(alias));

        return child.As<TProperty>();
    }

    protected TProperty GetAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (IPublishedContent) Content().Value(alias);

        return value.As<TProperty>();
    }
    
    protected IEnumerable<TProperty> GetDataListValueAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias) ?? Enumerable.Empty<BlockListItem>();

        return values.Cast<BlockListItem>().Select(x => x.Content.As<TProperty>(_content));
    }

    protected IEnumerable<TProperty> GetNestedAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias) ?? Enumerable.Empty<IPublishedElement>();

        return values.Cast<IPublishedElement>().Select(x => x.As<TProperty>(_content));
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
    
    protected TProperty GetStaticLookupByNameAs<TProperty>(Expression<Func<T, TProperty>> memberExpression)
    where TProperty : INamedLookup {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = Content().Value<string>(alias);

        return StaticLookups.GetAll<TProperty>().SingleOrDefault(x => x.Name.EqualsInvariant(value));
    }
    
    protected TProperty GetStaticLookupByIdAs<TProperty>(Expression<Func<T, TProperty>> memberExpression)
        where TProperty : INamedLookup {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = Content().Value<string>(alias);

        return StaticLookups.GetAll<TProperty>().SingleOrDefault(x => x.Id.EqualsInvariant(value));
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
            return publishedElement.As<TProperty>(_content);
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
