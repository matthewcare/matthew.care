using MatthewDotCare.XStatic.App;

namespace MatthewDotCare.XStatic.Git
{
    public static class GitExtensions
    {
        public static IDeployServiceBuilder AddGitDeployer(this IDeployServiceBuilder builder)
        {
            builder.AddDeployer(new GitDeployerDefinition(), (x) => new GitDeployer(x));

            return builder;
        }
    }
}