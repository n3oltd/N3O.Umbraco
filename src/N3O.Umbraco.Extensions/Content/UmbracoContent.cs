using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Content {
    public abstract class UmbracoContent<T> : Value, IUmbracoContent {
        [ValueIgnore]
        public virtual IPublishedContent Content { get; set; }

        protected TProperty GetAs<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
            var alias = AliasHelper<T>.PropertyAlias(memberExpression);
            var value = (IPublishedContent) Content.Value(alias);

            return value.As<TProperty>();
        }
    
        protected IEnumerable<TProperty> GetCollectionAs<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> memberExpression) {
            var alias = AliasHelper<T>.PropertyAlias(memberExpression);
            var values = (IEnumerable) Content.Value(alias);

            return values.Cast<IPublishedContent>().Select(x => x.As<TProperty>());
        }
    
        protected LocalDate GetLocalDate(Expression<Func<T, LocalDate>> memberExpression) {
            return GetConvertedValue<DateTime, LocalDate>(memberExpression, dt => dt.ToLocalDate());
        }
        
        protected TProperty GetValue<TProperty>(Expression<Func<T, TProperty>> memberExpression) {
            var alias = AliasHelper<T>.PropertyAlias(memberExpression);

            return Content.Value<TProperty>(alias);
        }
        
        protected TConverted GetConvertedValue<TProperty, TConverted>(Expression<Func<T, TConverted>> memberExpression,
                                                                      Func<TProperty, TConverted> convert) {
            var alias = AliasHelper<T>.PropertyAlias(memberExpression);

            return convert(Content.Value<TProperty>(alias));
        }
    }
}