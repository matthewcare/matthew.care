//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v10.1.0+3972538
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

namespace MatthewDotCare.Contracts.DocumentTypes
{
	/// <summary>Content Page</summary>
	[PublishedModel("contentPage")]
	public partial class ContentPage : PublishedContentModel, ICompositionBasePage
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		public new const string ModelTypeAlias = "contentPage";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedSnapshotAccessor publishedSnapshotAccessor)
			=> PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedSnapshotAccessor publishedSnapshotAccessor, Expression<Func<ContentPage, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(publishedSnapshotAccessor), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public ContentPage(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Page Title: The primary page name used in headings, SEO title & search results for the page
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("pageTitle")]
		public virtual string PageTitle => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetPageTitle(this, _publishedValueFallback);

		///<summary>
		/// Description: HTML Meta description
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("seoDescription")]
		public virtual string SeoDescription => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSeoDescription(this, _publishedValueFallback);

		///<summary>
		/// Indexing
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("seoIndexing")]
		public virtual string SeoIndexing => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSeoIndexing(this, _publishedValueFallback);

		///<summary>
		/// OG Meta Image: If this is left blank, then the base meta image set on the outer template will be used
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("seoOgMetaImage")]
		public virtual global::Umbraco.Cms.Core.Models.MediaWithCrops SeoOgMetaImage => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSeoOgMetaImage(this, _publishedValueFallback);

		///<summary>
		/// Site Map Change Frequency
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("seoSiteMapChangeFrequency")]
		public virtual string SeoSiteMapChangeFrequency => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSeoSiteMapChangeFrequency(this, _publishedValueFallback);

		///<summary>
		/// Site Map Priority
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[ImplementPropertyType("seoSiteMapPriority")]
		public virtual decimal SeoSiteMapPriority => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSeoSiteMapPriority(this, _publishedValueFallback);

		///<summary>
		/// Title: Overrides the page {title} (and OG:Title)- defaults to Page Title when blank
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("seoTitle")]
		public virtual string SeoTitle => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSeoTitle(this, _publishedValueFallback);

		///<summary>
		/// Site Map Display: Configures whether the current page and/or children are included in both the XML & HTML sitemap
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "10.1.0+3972538")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteMapDisplay")]
		public virtual string SiteMapDisplay => global::MatthewDotCare.Contracts.DocumentTypes.CompositionBasePage.GetSiteMapDisplay(this, _publishedValueFallback);
	}
}
