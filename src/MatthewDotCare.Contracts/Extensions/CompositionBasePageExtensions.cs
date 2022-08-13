using MatthewDotCare.Contracts.DocumentTypes;
using Umbraco.Extensions;

namespace MatthewDotCare.Contracts.Extensions;

public static class CompositionBasePageExtensions
{
    /// <summary>
    /// Gets the page title, or the page name if the title is left blank
    /// </summary>
    public static string PageTitleOrDefault(this ICompositionBasePage page) => page.PageTitle.OrIfNullOrWhiteSpace(page.Name);
}