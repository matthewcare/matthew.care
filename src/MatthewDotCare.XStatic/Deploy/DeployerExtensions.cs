using MatthewDotCare.XStatic.App;
using Microsoft.Extensions.DependencyInjection;

namespace MatthewDotCare.XStatic.Deploy
{
    public static class DeployerExtensions
    {
        public static IDeployServiceBuilder AddXStaticDeploy(this IServiceCollection services)
        {
            return new DeployServiceBuilder(services);
        }
    }
}