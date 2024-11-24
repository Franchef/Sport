
using DataPedanaHandler;
using Microsoft.Extensions.DependencyInjection;

namespace DatiPedanaUI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatiPedanaFeature(this IServiceCollection services)
        {
            return services
                .AddScoped<DatiPedanaUserControl>()
                .AddScoped<IDatiPedanaFileHandler, DatiPedanaFileHandler>();   
        }
    }
}
