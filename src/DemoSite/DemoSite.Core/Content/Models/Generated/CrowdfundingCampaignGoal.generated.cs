//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v13.4.1+d72fc5c
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
	// Mixin Content Type with alias "crowdfundingCampaignGoal"
	/// <summary>Campaign Goal</summary>
	public partial interface ICrowdfundingCampaignGoal : IPublishedElement
	{
		/// <summary>Amount</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		decimal Amount { get; }

		/// <summary>Location</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Content.FundDimension1Value FundDimension1 { get; }

		/// <summary>Theme</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Content.FundDimension2Value FundDimension2 { get; }

		/// <summary>Stipulation</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Content.FundDimension3Value FundDimension3 { get; }

		/// <summary>Price Handles</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.PriceHandle> PriceHandles { get; }

		/// <summary>Tags</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingCampaignTag> Tags { get; }

		/// <summary>Title</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string Title { get; }
	}

	/// <summary>Campaign Goal</summary>
	[PublishedModel("crowdfundingCampaignGoal")]
	public partial class CrowdfundingCampaignGoal : PublishedElementModel, ICrowdfundingCampaignGoal
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public new const string ModelTypeAlias = "crowdfundingCampaignGoal";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<CrowdfundingCampaignGoal, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public CrowdfundingCampaignGoal(IPublishedElement content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Amount
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[ImplementPropertyType("amount")]
		public virtual decimal Amount => GetAmount(this, _publishedValueFallback);

		/// <summary>Static getter for Amount</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public static decimal GetAmount(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<decimal>(publishedValueFallback, "amount");

		///<summary>
		/// Location
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("fundDimension1")]
		public virtual global::DemoSite.Content.FundDimension1Value FundDimension1 => GetFundDimension1(this, _publishedValueFallback);

		/// <summary>Static getter for Location</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Content.FundDimension1Value GetFundDimension1(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Content.FundDimension1Value>(publishedValueFallback, "fundDimension1");

		///<summary>
		/// Theme
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("fundDimension2")]
		public virtual global::DemoSite.Content.FundDimension2Value FundDimension2 => GetFundDimension2(this, _publishedValueFallback);

		/// <summary>Static getter for Theme</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Content.FundDimension2Value GetFundDimension2(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Content.FundDimension2Value>(publishedValueFallback, "fundDimension2");

		///<summary>
		/// Stipulation
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("fundDimension3")]
		public virtual global::DemoSite.Content.FundDimension3Value FundDimension3 => GetFundDimension3(this, _publishedValueFallback);

		/// <summary>Static getter for Stipulation</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Content.FundDimension3Value GetFundDimension3(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Content.FundDimension3Value>(publishedValueFallback, "fundDimension3");

		///<summary>
		/// Price Handles
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("priceHandles")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.PriceHandle> PriceHandles => GetPriceHandles(this, _publishedValueFallback);

		/// <summary>Static getter for Price Handles</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.PriceHandle> GetPriceHandles(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.PriceHandle>>(publishedValueFallback, "priceHandles");

		///<summary>
		/// Tags
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tags")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingCampaignTag> Tags => GetTags(this, _publishedValueFallback);

		/// <summary>Static getter for Tags</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingCampaignTag> GetTags(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingCampaignTag>>(publishedValueFallback, "tags");

		///<summary>
		/// Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("title")]
		public virtual string Title => GetTitle(this, _publishedValueFallback);

		/// <summary>Static getter for Title</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetTitle(ICrowdfundingCampaignGoal that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "title");
	}
}
