//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb
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

namespace DemoSite.Core.Content
{
	// Mixin Content Type with alias "donationOption"
	/// <summary>Donation Option</summary>
	public partial interface IDonationOption : IPublishedContent
	{
		/// <summary>Location</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Core.Content.FundDimension1Option Dimension1 { get; }

		/// <summary>Theme</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Core.Content.FundDimension2Option Dimension2 { get; }

		/// <summary>Stipulation</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::DemoSite.Core.Content.FundDimension3Option Dimension3 { get; }

		/// <summary>Hide</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		bool HideDonation { get; }

		/// <summary>Hide</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		bool HideRegularGiving { get; }

		/// <summary>Show Quantity</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		bool ShowQuantity { get; }
	}

	/// <summary>Donation Option</summary>
	[PublishedModel("donationOption")]
	public partial class DonationOption : PublishedContentModel, IDonationOption
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		public new const string ModelTypeAlias = "donationOption";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<DonationOption, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public DonationOption(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Location
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension1")]
		public virtual global::DemoSite.Core.Content.FundDimension1Option Dimension1 => GetDimension1(this, _publishedValueFallback);

		/// <summary>Static getter for Location</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Core.Content.FundDimension1Option GetDimension1(IDonationOption that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Core.Content.FundDimension1Option>(publishedValueFallback, "dimension1");

		///<summary>
		/// Theme
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension2")]
		public virtual global::DemoSite.Core.Content.FundDimension2Option Dimension2 => GetDimension2(this, _publishedValueFallback);

		/// <summary>Static getter for Theme</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Core.Content.FundDimension2Option GetDimension2(IDonationOption that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Core.Content.FundDimension2Option>(publishedValueFallback, "dimension2");

		///<summary>
		/// Stipulation
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("dimension3")]
		public virtual global::DemoSite.Core.Content.FundDimension3Option Dimension3 => GetDimension3(this, _publishedValueFallback);

		/// <summary>Static getter for Stipulation</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::DemoSite.Core.Content.FundDimension3Option GetDimension3(IDonationOption that, IPublishedValueFallback publishedValueFallback) => that.Value<global::DemoSite.Core.Content.FundDimension3Option>(publishedValueFallback, "dimension3");

		///<summary>
		/// Hide
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[ImplementPropertyType("hideDonation")]
		public virtual bool HideDonation => GetHideDonation(this, _publishedValueFallback);

		/// <summary>Static getter for Hide</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		public static bool GetHideDonation(IDonationOption that, IPublishedValueFallback publishedValueFallback) => that.Value<bool>(publishedValueFallback, "hideDonation");

		///<summary>
		/// Hide
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[ImplementPropertyType("hideRegularGiving")]
		public virtual bool HideRegularGiving => GetHideRegularGiving(this, _publishedValueFallback);

		/// <summary>Static getter for Hide</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		public static bool GetHideRegularGiving(IDonationOption that, IPublishedValueFallback publishedValueFallback) => that.Value<bool>(publishedValueFallback, "hideRegularGiving");

		///<summary>
		/// Show Quantity
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		[ImplementPropertyType("showQuantity")]
		public virtual bool ShowQuantity => GetShowQuantity(this, _publishedValueFallback);

		/// <summary>Static getter for Show Quantity</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.2.0+763cb70e677ac0c85557b19b5df09eccfa1b9dfb")]
		public static bool GetShowQuantity(IDonationOption that, IPublishedValueFallback publishedValueFallback) => that.Value<bool>(publishedValueFallback, "showQuantity");
	}
}
