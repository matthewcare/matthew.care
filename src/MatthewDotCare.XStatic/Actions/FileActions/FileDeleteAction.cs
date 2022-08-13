using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Helpers;

namespace MatthewDotCare.XStatic.Actions.FileActions
{
    [XStaticEditableField("FilePath")]
    public class FileDeleteAction : IPostGenerationAction
    {
        private readonly IStaticSiteStorer _staticSiteStorer;

        public FileDeleteAction(IStaticSiteStorer staticSiteStorer)
        {
            _staticSiteStorer = staticSiteStorer;
        }

        public virtual async Task<XStaticResult> RunAction(int staticSiteId, Dictionary<string, string> parameters)
        {
            var existingFilePath = parameters["FilePath"];

            return await DeleteFile(staticSiteId, existingFilePath);
        }

        protected virtual async Task<XStaticResult> DeleteFile(int staticSiteId, string existingFilePath)
        {
            var absoluteFilePath = FileHelpers.PathCombine(_staticSiteStorer.GetStorageLocationOfSite(staticSiteId), existingFilePath);

            try
            {
                await _staticSiteStorer.DeleteFile(absoluteFilePath);
            }
            catch (Exception e)
            {
                return XStaticResult.Error("Error running file delete action", e);
            }

            return XStaticResult.Success();
        }
    }
}