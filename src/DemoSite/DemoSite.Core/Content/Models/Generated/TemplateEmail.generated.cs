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
	// Mixin Content Type with alias "templateEmail"
	/// <summary>Template Email</summary>
	public partial interface ITemplateEmail : IPublishedContent
	{
		/// <summary>Body</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string Body { get; }

		/// <summary>From Email</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string FromEmail { get; }

		/// <summary>From Name</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string FromName { get; }

		/// <summary>Subject</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		string Subject { get; }
	}

	/// <summary>Template Email</summary>
	[PublishedModel("templateEmail")]
	public partial class TemplateEmail : PublishedContentModel, ITemplateEmail
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		public new const string ModelTypeAlias = "templateEmail";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<TemplateEmail, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public TemplateEmail(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Body
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("body")]
		public virtual string Body => GetBody(this, _publishedValueFallback);

		/// <summary>Static getter for Body</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetBody(ITemplateEmail that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "body");

		///<summary>
		/// From Email
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("fromEmail")]
		public virtual string FromEmail => GetFromEmail(this, _publishedValueFallback);

		/// <summary>Static getter for From Email</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetFromEmail(ITemplateEmail that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "fromEmail");

		///<summary>
		/// From Name
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("fromName")]
		public virtual string FromName => GetFromName(this, _publishedValueFallback);

		/// <summary>Static getter for From Name</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetFromName(ITemplateEmail that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "fromName");

		///<summary>
		/// Subject
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("subject")]
		public virtual string Subject => GetSubject(this, _publishedValueFallback);

		/// <summary>Static getter for Subject</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "9.3.0-rc+7971f36b78e333aa61cb94e9fc3003b70459c6ff")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static string GetSubject(ITemplateEmail that, IPublishedValueFallback publishedValueFallback) => that.Value<string>(publishedValueFallback, "subject");
	}
}
