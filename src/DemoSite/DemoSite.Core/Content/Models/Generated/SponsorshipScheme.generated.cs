//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v11.1.0+bad9148
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
	/// <summary>Sponsorship Scheme</summary>
	[PublishedModel("sponsorshipScheme")]
	public partial class SponsorshipScheme : PublishedContentModel, IFundDimensionsOptions
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		public new const string ModelTypeAlias = "sponsorshipScheme";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<SponsorshipScheme, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public SponsorshipScheme(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Allowed Durations
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("allowedDurations")]
		public virtual global::System.Collections.Generic.List<global::N3O.Umbraco.Giving.Lookups.SponsorshipDuration> AllowedDurations => this.Value<global::System.Collections.Generic.List<global::N3O.Umbraco.Giving.Lookups.SponsorshipDuration>>(_publishedValueFallback, "allowedDurations");

		///<summary>
		/// Allowed Giving Types
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("allowedGivingTypes")]
		public virtual global::System.Collections.Generic.List<global::N3O.Umbraco.Giving.Lookups.GivingType> AllowedGivingTypes => this.Value<global::System.Collections.Generic.List<global::N3O.Umbraco.Giving.Lookups.GivingType>>(_publishedValueFallback, "allowedGivingTypes");

		///<summary>
		/// Locations
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension1Options")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FundDimension1Value> Dimension1Options => global::DemoSite.Content.FundDimensionsOptions.GetDimension1Options(this, _publishedValueFallback);

		///<summary>
		/// Themes
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension2Options")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FundDimension2Value> Dimension2Options => global::DemoSite.Content.FundDimensionsOptions.GetDimension2Options(this, _publishedValueFallback);

		///<summary>
		/// Stipulations
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "11.1.0+bad9148")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension3Options")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.FundDimension3Value> Dimension3Options => global::DemoSite.Content.FundDimensionsOptions.GetDimension3Options(this, _publishedValueFallback);
	}
}
