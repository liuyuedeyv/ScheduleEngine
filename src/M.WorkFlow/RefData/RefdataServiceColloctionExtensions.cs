using System;
using Microsoft.Extensions.DependencyInjection;

namespace M.WFDesigner.RefData
{
    public static class RefdataServiceColloctionExtensions
    {
        public static IServiceCollection AddRefdata(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            //服务
            services.AddSingleton<WFServiceTableRefdataRepository>(); 
             
            return services;
        }
    }
}
