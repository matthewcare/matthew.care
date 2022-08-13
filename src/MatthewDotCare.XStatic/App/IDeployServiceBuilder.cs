using MatthewDotCare.XStatic.Deploy;

namespace MatthewDotCare.XStatic.App
{
    public interface IDeployServiceBuilder
    {
        IDeployServiceBuilder AddDeployer(IDeployerDefinition definition, Func<Dictionary<string, string>, IDeployer> deployer);

        void Build();
    }
}