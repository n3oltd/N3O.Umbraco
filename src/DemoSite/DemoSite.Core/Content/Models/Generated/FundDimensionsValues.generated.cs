//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v13.5.0+a6c5581
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
	// Mixin Content Type with alias "fundDimensionsValues"
	/// <summary>Fund Dimensions Values</summary>
	public partial interface IFundDimensionsValues : IPublishedElement
	{
		/// <summary>Location</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Content.FundDimension1Value Dimension1 { get; }

		/// <summary>Theme</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Content.FundDimension2Value Dimension2 { get; }

		/// <summary>Stipulation</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Content.FundDimension3Value Dimension3 { get; }
	}

	/// <summary>Fund Dimensions Values</summary>
	[PublishedModel("fundDimensionsValues")]
	public partial class FundDimensionsValues : PublishedElementModel, IFundDimensionsValues
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		public new const string ModelTypeAlias = "fundDimensionsValues";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<FundDimensionsValues, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public FundDimensionsValues(IPublishedElement content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Location
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension1")]
		public virtual global::DemoSite.Content.FundDimension1Value Dimension1 => GetDimension1(this, _publishedValueFallback);

		/// <summary>Static getter for Location</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Content.FundDimension1Value GetDimension1(IFundDimensionsValues that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Content.FundDimension1Value>(publishedValueFallback, "dimension1");

		///<summary>
		/// Theme
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension2")]
		public virtual global::DemoSite.Content.FundDimension2Value Dimension2 => GetDimension2(this, _publishedValueFallback);

		/// <summary>Static getter for Theme</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Content.FundDimension2Value GetDimension2(IFundDimensionsValues that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Content.FundDimension2Value>(publishedValueFallback, "dimension2");

		///<summary>
		/// Stipulation
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension3")]
		public virtual global::DemoSite.Content.FundDimension3Value Dimension3 => GetDimension3(this, _publishedValueFallback);

		/// <summary>Static getter for Stipulation</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.5.0+a6c5581")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Content.FundDimension3Value GetDimension3(IFundDimensionsValues that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Content.FundDimension3Value>(publishedValueFallback, "dimension3");
	}
}
