using Best.Practices.Core.Application.Services;
using Best.Practices.Core.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Best.Practices.Core.Configurations
{
    public static class ServiceCollectionExtentions
    {
        public static void MapDefaultApplicationServices(this IServiceCollection service)
        {
            service.AddScoped<ITokenAuthentication, TokenAuthentication>();
        }
    }
}