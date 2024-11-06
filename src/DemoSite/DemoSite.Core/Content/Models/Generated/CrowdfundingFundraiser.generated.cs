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
	/// <summary>Fundraiser</summary>
	[PublishedModel("crowdfundingFundraiser")]
	public partial class CrowdfundingFundraiser : PublishedContentModel, ICrowdfundingCrowdfunder
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public new const string ModelTypeAlias = "crowdfundingFundraiser";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<CrowdfundingFundraiser, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public CrowdfundingFundraiser(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Account
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("account")]
		public virtual string Account => this.Value<string>(_publishedValueFallback, "account");

		///<summary>
		/// Campaign
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("campaign")]
		public virtual global::DemoSite.Content.CrowdfundingCampaign Campaign => this.Value<global::DemoSite.Content.CrowdfundingCampaign>(_publishedValueFallback, "campaign");

		///<summary>
		/// Emails
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("emails")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingFundraiserNotificationEmail> Emails => this.Value<global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingFundraiserNotificationEmail>>(_publishedValueFallback, "emails");

		///<summary>
		/// Owner
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("owner")]
		public virtual global::DemoSite.Content.Member Owner => this.Value<global::DemoSite.Content.Member>(_publishedValueFallback, "owner");

		///<summary>
		/// Slug
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("slug")]
		public virtual string Slug => this.Value<string>(_publishedValueFallback, "slug");

		///<summary>
		/// Background Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("backgroundImage")]
		public virtual global::N3O.Umbraco.Cropper.Models.CroppedImage BackgroundImage => global::DemoSite.Content.CrowdfundingCrowdfunder.GetBackgroundImage(this, _publishedValueFallback);

		///<summary>
		/// Body
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("body")]
		public virtual global::Umbraco.Cms.Core.Strings.IHtmlEncodedString Body => global::DemoSite.Content.CrowdfundingCrowdfunder.GetBody(this, _publishedValueFallback);

		///<summary>
		/// Currency
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("currency")]
		public virtual global::N3O.Umbraco.Financial.Currency Currency => global::DemoSite.Content.CrowdfundingCrowdfunder.GetCurrency(this, _publishedValueFallback);

		///<summary>
		/// Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("description")]
		public virtual string Description => global::DemoSite.Content.CrowdfundingCrowdfunder.GetDescription(this, _publishedValueFallback);

		///<summary>
		/// Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("displayName")]
		public virtual string DisplayName => global::DemoSite.Content.CrowdfundingCrowdfunder.GetDisplayName(this, _publishedValueFallback);

		///<summary>
		/// Goals
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("goals")]
		public virtual global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedElement> Goals => global::DemoSite.Content.CrowdfundingCrowdfunder.GetGoals(this, _publishedValueFallback);

		///<summary>
		/// Hero Images
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("heroImages")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingHeroImage> HeroImages => global::DemoSite.Content.CrowdfundingCrowdfunder.GetHeroImages(this, _publishedValueFallback);

		///<summary>
		/// Open Graph Image
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("openGraphImagePath")]
		public virtual string OpenGraphImagePath => global::DemoSite.Content.CrowdfundingCrowdfunder.GetOpenGraphImagePath(this, _publishedValueFallback);

		///<summary>
		/// Status
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("status")]
		public virtual string Status => global::DemoSite.Content.CrowdfundingCrowdfunder.GetStatus(this, _publishedValueFallback);

		///<summary>
		/// Tags
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("tags")]
		public virtual global::System.Collections.Generic.IEnumerable<global::DemoSite.Content.CrowdfundingTag> Tags => global::DemoSite.Content.CrowdfundingCrowdfunder.GetTags(this, _publishedValueFallback);

		///<summary>
		/// Toggle Status
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "13.4.1+d72fc5c")]
		[ImplementPropertyType("toggleStatus")]
		public virtual bool ToggleStatus => global::DemoSite.Content.CrowdfundingCrowdfunder.GetToggleStatus(this, _publishedValueFallback);
	}
}
