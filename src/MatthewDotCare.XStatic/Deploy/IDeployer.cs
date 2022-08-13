namespace MatthewDotCare.XStatic.Deploy
{
    public interface IDeployer
    {
        Task<XStaticResult> DeployWholeSite(string folderPath);
    }
}