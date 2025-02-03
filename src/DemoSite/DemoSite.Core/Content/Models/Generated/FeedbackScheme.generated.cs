//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v13.6.0+b9837ac
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace DemoSite.Content
{
	/// <summary>Feedback Scheme</summary>
	[PublishedModel("feedbackScheme")]
	public partial class FeedbackScheme : PublishedContentModel, IFundDimensionsOptions, IPrice, IPricingRules
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		public new const string ModelTypeAlias = "feedbackScheme";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<FeedbackScheme, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public FeedbackScheme(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Allowed Giving Types
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("allowedGivingTypes")]
		public virtual global::System.Collections.Generic.List<string> AllowedGivingTypes => this.Value<global::System.Collections.Generic.List<string>>(_publishedValueFallback, "allowedGivingTypes");

		///<summary>
		/// Custom Fields
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("customFields")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FeedbackCustomFieldDefinition> CustomFields => this.Value<global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FeedbackCustomFieldDefinition>>(_publishedValueFallback, "customFields");

		///<summary>
		/// Locations
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension1Options")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FundDimension1Value> Dimension1Options => global::DemoSite.Content.FundDimensionsOptions.GetDimension1Options(this, _publishedValueFallback);

		///<summary>
		/// Themes
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension2Options")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FundDimension2Value> Dimension2Options => global::DemoSite.Content.FundDimensionsOptions.GetDimension2Options(this, _publishedValueFallback);

		///<summary>
		/// Stipulations
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension3Options")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FundDimension3Value> Dimension3Options => global::DemoSite.Content.FundDimensionsOptions.GetDimension3Options(this, _publishedValueFallback);

		///<summary>
		/// Price
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("priceAmount")]
		public virtual decimal PriceAmount => global::DemoSite.Content.Price.GetPriceAmount(this, _publishedValueFallback);

		///<summary>
		/// Price Locked
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[ImplementPropertyType("priceLocked")]
		public virtual bool PriceLocked => global::DemoSite.Content.Price.GetPriceLocked(this, _publishedValueFallback);

		///<summary>
		/// Price Rules
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.6.0+b9837ac")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("priceRules")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.PricingRule> PriceRules => global::DemoSite.Content.PricingRules.GetPriceRules(this, _publishedValueFallback);
	}
}
