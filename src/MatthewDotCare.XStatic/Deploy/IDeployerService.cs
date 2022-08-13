namespace MatthewDotCare.XStatic.Deploy
{
    public interface IDeployerService
    {
        IDeployerDefinition GetDefinition(string id);

        IEnumerable<IDeployerDefinition> GetDefinitions();

        IDeployer GetDeployer(string deployerKey, Dictionary<string, string> properties);
    }
}
