using Umbraco.Cms.Core.IO;

namespace MatthewDotCare.Umbraco.ViewContent;

public class ViewContentProvider : IDefaultViewContentProvider
{
    public string GetDefaultFileContent(string? layoutPageAlias = null, string? modelClassName = null,
        string? modelNamespace = null, string? modelNamespaceAlias = null) => $"@inherits UmbracoViewPage<{modelClassName}>";
}