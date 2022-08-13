using MatthewDotCare.XStatic.Generator.Storage;
using MatthewDotCare.XStatic.Helpers;

namespace MatthewDotCare.XStatic.Actions.FileActions
{
    [XStaticEditableField("FilePath")]
    [XStaticEditableField("NewFilePath")]
    public class FileRenameAction : IPostGenerationAction
    {
        private readonly IStaticSiteStorer _staticSiteStorer;

        public FileRenameAction(IStaticSiteStorer staticSiteStorer)
        {
            _staticSiteStorer = staticSiteStorer;
        }

        public virtual async Task<XStaticResult> RunAction(int staticSiteId, Dictionary<string, string> parameters)
        {
            var existingFilePath = parameters["FilePath"];
            var newFilePath = parameters["NewFilePath"];

            return await MoveFile(staticSiteId, existingFilePath, newFilePath);
        }

        protected virtual async Task<XStaticResult> MoveFile(int staticSiteId, string existingFilePath, string newFilePath)
        {
            var absoluteFilePath = FileHelpers.PathCombine(_staticSiteStorer.GetStorageLocationOfSite(staticSiteId), existingFilePath);

            try
            {
                await _staticSiteStorer.MoveFile(staticSiteId.ToString(), absoluteFilePath, newFilePath);
            }
            catch (Exception e)
            {
                return XStaticResult.Error("Error running file rename action", e);
            }

            return XStaticResult.Success();
        }
    }
}