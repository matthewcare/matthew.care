using MatthewDotCare.XStatic.Deploy;

namespace MatthewDotCare.XStatic.Git
{
    public class GitAutoInstaller : IDeployerAutoInstaller
    {
        public IDeployerDefinition Definition => new GitDeployerDefinition();

        public Func<Dictionary<string, string>, IDeployer> Constructor => (x) => new GitDeployer(x);
    }
}