using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Helpers;

namespace MatthewDotCare.XStatic.Actions.FileActions
{
    [XStaticEditableField("FilePath")]
    [XStaticEditableField("NewFilePath")]
    public class FileCopyAction : IPostGenerationAction
    {
        private readonly IStaticSiteStorer _staticSiteStorer;

        public FileCopyAction(IStaticSiteStorer staticSiteStorer)
        {
            _staticSiteStorer = staticSiteStorer;
        }

        public virtual async Task<XStaticResult> RunAction(int staticSiteId, Dictionary<string, string> parameters)
        {
            var existingFilePath = parameters["FilePath"];
            var newFilePath = parameters["NewFilePath"];

            return await CopyFile(staticSiteId, existingFilePath, newFilePath);
        }

        protected virtual async Task<XStaticResult> CopyFile(int staticSiteId, string existingFilePath, string newFilePath)
        {
            var absoluteFilePath = FileHelpers.PathCombine(_staticSiteStorer.GetStorageLocationOfSite(staticSiteId), existingFilePath);

            try
            {
                await _staticSiteStorer.CopyFile(staticSiteId.ToString(), absoluteFilePath, newFilePath);
            }
            catch (Exception e)
            {
                return XStaticResult.Error("Error running file copy action", e);
            }

            return XStaticResult.Success();
        }
    }
}