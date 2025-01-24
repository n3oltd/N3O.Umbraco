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
	// Mixin Content Type with alias "crowdfundingCrowdfunder"
	/// <summary>Crowdfunder</summary>
	public partial interface ICrowdfundingCrowdfunder : IPublishedContent
	{
		/// <summary>Background Image</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::N3O.Umbraco.Cropper.Models.CroppedImage BackgroundImage { get; }

		/// <summary>Body</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::Umbraco.Cms.Core.Strings.IHtmlEncodedString Body { get; }

		/// <summary>Currency</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::N3O.Umbraco.Financial.Currency Currency { get; }

		/// <summary>Description</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string Description { get; }

		/// <summary>Title</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string DisplayName { get; }

		/// <summary>Error</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string Error { get; }

		/// <summary>Goals</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedElement> Goals { get; }

		/// <summary>Goals Description</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		object GoalsDescription { get; }

		/// <summary>Hero Images</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingHeroImage> HeroImages { get; }

		/// <summary>Open Graph Image</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string OpenGraphImagePath { get; }

		/// <summary>Status</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string Status { get; }

		/// <summary>Tags</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingTag> Tags { get; }

		/// <summary>Toggle Status</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		bool ToggleStatus { get; }
	}

	/// <summary>Crowdfunder</summary>
	[PublishedModel("crowdfundingCrowdfunder")]
	public partial class CrowdfundingCrowdfunder : PublishedContentModel, ICrowdfundingCrowdfunder
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public new const string ModelTypeAlias = "crowdfundingCrowdfunder";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<CrowdfundingCrowdfunder, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public CrowdfundingCrowdfunder(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Background Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("backgroundImage")]
		public virtual global::N3O.Umbraco.Cropper.Models.CroppedImage BackgroundImage => GetBackgroundImage(this, _publishedValueFallback);

		/// <summary>Static getter for Background Image</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::N3O.Umbraco.Cropper.Models.CroppedImage GetBackgroundImage(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<global::N3O.Umbraco.Cropper.Models.CroppedImage>(publishedValueFallback, "backgroundImage");

		///<summary>
		/// Body
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("body")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString Body => GetBody(this, _publishedValueFallback);

		/// <summary>Static getter for Body</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::Umbraco.Cms.Core.Strings.IHtmlEncodedString GetBody(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<global::Umbraco.Cms.Core.Strings.IHtmlEncodedString>(publishedValueFallback, "body");

		///<summary>
		/// Currency
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("currency")]
		public virtual global::N3O.Umbraco.Financial.Currency Currency => GetCurrency(this, _publishedValueFallback);

		/// <summary>Static getter for Currency</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::N3O.Umbraco.Financial.Currency GetCurrency(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<global::N3O.Umbraco.Financial.Currency>(publishedValueFallback, "currency");

		///<summary>
		/// Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("description")]
		public virtual string Description => GetDescription(this, _publishedValueFallback);

		/// <summary>Static getter for Description</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetDescription(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "description");

		///<summary>
		/// Title
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("displayName")]
		public virtual string DisplayName => GetDisplayName(this, _publishedValueFallback);

		/// <summary>Static getter for Title</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetDisplayName(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "displayName");

		///<summary>
		/// Error
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("error")]
		public virtual string Error => GetError(this, _publishedValueFallback);

		/// <summary>Static getter for Error</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetError(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "error");

		///<summary>
		/// Goals
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("goals")]
		public virtual global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedElement> Goals => GetGoals(this, _publishedValueFallback);

		/// <summary>Static getter for Goals</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedElement> GetGoals(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedElement>>(publishedValueFallback, "goals");

		///<summary>
		/// Goals Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("goalsDescription")]
		public virtual object GoalsDescription => GetGoalsDescription(this, _publishedValueFallback);

		/// <summary>Static getter for Goals Description</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static object GetGoalsDescription(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value(publishedValueFallback, "goalsDescription");

		///<summary>
		/// Hero Images
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heroImages")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingHeroImage> HeroImages => GetHeroImages(this, _publishedValueFallback);

		/// <summary>Static getter for Hero Images</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingHeroImage> GetHeroImages(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingHeroImage>>(publishedValueFallback, "heroImages");

		///<summary>
		/// Open Graph Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("openGraphImagePath")]
		public virtual string OpenGraphImagePath => GetOpenGraphImagePath(this, _publishedValueFallback);

		/// <summary>Static getter for Open Graph Image</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetOpenGraphImagePath(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "openGraphImagePath");

		///<summary>
		/// Status
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("status")]
		public virtual string Status => GetStatus(this, _publishedValueFallback);

		/// <summary>Static getter for Status</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetStatus(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "status");

		///<summary>
		/// Tags
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tags")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingTag> Tags => GetTags(this, _publishedValueFallback);

		/// <summary>Static getter for Tags</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingTag> GetTags(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingTag>>(publishedValueFallback, "tags");

		///<summary>
		/// Toggle Status
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[ImplementPropertyType("toggleStatus")]
		public virtual bool ToggleStatus => GetToggleStatus(this, _publishedValueFallback);

		/// <summary>Static getter for Toggle Status</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public static bool GetToggleStatus(ICrowdfundingCrowdfunder that, IPublishedValueFallback publishedValueFallback) => that.Value<bool>(publishedValueFallback, "toggleStatus");
	}
}
