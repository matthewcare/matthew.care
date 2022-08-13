using Microsoft.Extensions.DependencyInjection;

namespace MatthewDotCare.XStatic.UmbrcoContentApi
{
    public static class UmbracoContentApiExtensions
    {
        public static IServiceCollection AddUmbracoContentApi(this IServiceCollection services)
        {
            services.AddSingleton<UmbracoContentApiGenerator>();

            return services;
        }
    }
}