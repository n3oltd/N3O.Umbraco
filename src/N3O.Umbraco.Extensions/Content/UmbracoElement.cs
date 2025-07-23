using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
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

    public virtual void SetContent(IPublishedElement content, IPublishedContent parent) {
        _content = content;
        _parent = parent;
    }
    
    public virtual void SetVariationContext(VariationContext variationContext) => VariationContext = variationContext;

    protected VariationContext VariationContext { get; private set; }

    protected TProperty GetAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = (IPublishedContent) Content().Value(alias, VariationContext?.Culture, VariationContext?.Segment);

        return value.As<TProperty>();
    }

    protected IEnumerable<TProperty> GetCollectionAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias, VariationContext?.Culture, VariationContext?.Segment);

        return values.Cast<IPublishedContent>().Select(x => x.As<TProperty>());
    }
    
    public string GetLocalizedString<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var text = (string) Content().GetProperty(alias).GetValue(VariationContext?.Culture, VariationContext?.Segment);
        
        return StringLocalizer.Instance.Get(GetType().GetFriendlyName(), alias, text);
    }
    
    protected TLookup GetLookup<TLookup>(ILookups lookups, string propertyAlias)
        where TLookup : ILookup {
        var propertyValue = Content().GetProperty(propertyAlias)?.GetValue(VariationContext?.Culture, VariationContext?.Segment);

        TLookup lookup;
        
        if (propertyValue is TLookup lookupValue) {
            lookup =  lookupValue;
        } else if (propertyValue is IPublishedContent publishedContent) {
            lookup = lookups.FindById<TLookup>(LookupContent.GetId(publishedContent));
        } else {
            throw new Exception("Lookup properties must either be a datalist or picker");
        }

        return lookup;
    }
    
    protected IEnumerable<TLookup> GetLookups<TLookup>(ILookups lookups, string propertyAlias)
        where TLookup : ILookup {
        var propertyValues = Content().GetProperty(propertyAlias)?.GetValue(VariationContext?.Culture, VariationContext?.Segment);

        var tLookups = new List<TLookup>();
        
        if (propertyValues is IEnumerable<TLookup> lookupValues) {
            tLookups.AddRange(lookupValues);
        } else if (propertyValues is IEnumerable<IPublishedContent> publishedContents) {
            var publishedLookupValues = publishedContents.Select(x => lookups.FindById<TLookup>(LookupContent.GetId(x)));
            
            tLookups.AddRange(publishedLookupValues);
        } else {
            throw new Exception("Lookup properties must either be a datalist or picker");
        }

        return tLookups;
    }
    
    protected IEnumerable<TProperty> GetNestedAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias, VariationContext?.Culture, VariationContext?.Segment)
                     ?? Enumerable.Empty<IPublishedElement>();

        return values.Cast<IPublishedElement>().Select(x => x.As<TProperty>(_parent));
    }
    
    protected TProperty GetPickedAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var value = Content().Value(alias, VariationContext?.Culture, VariationContext?.Segment);

        if (value is TProperty typedValue) {
            return typedValue;
        } else {
            return ((IPublishedContent) value).As<TProperty>();
        }
    }
    
    protected IEnumerable<TProperty> GetPickedAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
        var alias = AliasHelper<T>.PropertyAlias(memberExpression);
        var values = (IEnumerable) Content().Value(alias, VariationContext?.Culture, VariationContext?.Segment)
                     ?? Enumerable.Empty<IPublishedContent>();

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

        var propertyValue = property.GetValue(VariationContext?.Culture, VariationContext?.Segment);

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

        return convert(Content().Value<TProperty>(alias, VariationContext?.Culture, VariationContext?.Segment));
    }
}
