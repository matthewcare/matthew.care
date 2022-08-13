using MatthewDotCare.XStatic.Generator;
using MatthewDotCare.XStatic.Generator.Ssl;
using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Generator.Transformers;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Hosting;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using UmbracoContentApi.Core.Resolvers;

namespace MatthewDotCare.XStatic.UmbrcoContentApi
{
    public class UmbracoContentApiGenerator : GeneratorBase
    {
        private readonly Lazy<IContentResolver> _contentResolver;

        public UmbracoContentApiGenerator(IUmbracoContextFactory umbracoContextFactory,
            IPublishedUrlProvider publishedUrlProvider,
            IStaticSiteStorer storer,
            IImageCropNameGenerator imageCropNameGenerator,
            MediaFileManager mediaFileSystem,
            IHostingEnvironment hostingEnvironment,
            Lazy<IContentResolver> contentResolver)
            : base(umbracoContextFactory, publishedUrlProvider, storer, imageCropNameGenerator, mediaFileSystem, hostingEnvironment)
        {
            _contentResolver = contentResolver;
        }

        public override async Task<string> GeneratePage(int id, int staticSiteId, IFileNameGenerator fileNamer, IEnumerable<ITransformer> transformers = null)
        {
            SslTruster.TrustSslIfAppSettingConfigured();

            var node = GetNode(id);

            if (node == null)
            {
                return null;
            }

            var url = node.Url(_publishedUrlProvider, mode: UrlMode.Relative);

            var model = _contentResolver.Value.ResolveContent(node);

            var fileData = JsonConvert.SerializeObject(model);

            if (fileData == null)
            {
                return null;
            }

            var transformedData = RunTransformers(fileData, transformers);

            var filePath = fileNamer.GetFilePartialPath(url);

            var generatedFileLocation = await Store(staticSiteId, filePath, transformedData);

            return generatedFileLocation;
        }
    }
}