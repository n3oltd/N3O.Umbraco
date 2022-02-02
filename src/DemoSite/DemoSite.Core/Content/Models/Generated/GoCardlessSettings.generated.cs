//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff
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
	/// <summary>GoCardless Settings</summary>
	[PublishedModel("goCardlessSettings")]
	public partial class GoCardlessSettings : PublishedContentModel, IPaymentMethodSettings
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		public new const string ModelTypeAlias = "goCardlessSettings";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<GoCardlessSettings, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public GoCardlessSettings(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Access Token
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("productionAccessToken")]
		public virtual string ProductionAccessToken => this.Value<string>(_publishedValueFallback, "productionAccessToken");

		///<summary>
		/// Access Token
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("sandboxAccessToken")]
		public virtual string SandboxAccessToken => this.Value<string>(_publishedValueFallback, "sandboxAccessToken");

		///<summary>
		/// Transaction Description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("transactionDescription")]
		public virtual string TransactionDescription => global::DemoSite.Core.Content.PaymentMethodSettings.GetTransactionDescription(this, _publishedValueFallback);

		///<summary>
		/// Transaction ID
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("transactionId")]
		public virtual string TransactionId => global::DemoSite.Core.Content.PaymentMethodSettings.GetTransactionId(this, _publishedValueFallback);
	}
}
