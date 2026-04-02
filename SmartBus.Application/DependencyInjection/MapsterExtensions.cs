using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.DependencyInjection
{
    public static class MapsterExtensions
    {
        public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MapsterExtensions).Assembly); // scans Application assembly

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;

        }
    }
}
