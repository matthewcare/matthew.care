using MatthewDotCare.XStatic.Deploy;

namespace MatthewDotCare.XStatic.Git
{
    public class GitDeployerDefinition : IDeployerDefinition
    {
        public string Id => GitDeployer.DeployerKey;

        public string Name => "Git";

        public string Help => "First create an empty git repo. This deployer will clone the remote and then push changes back on each deploy.";

        public IEnumerable<string> Fields => new[]
        {
            "RemoteUrl",
            "Email",
            "Username",
            "Password",
            "Branch"
        };
    }
}