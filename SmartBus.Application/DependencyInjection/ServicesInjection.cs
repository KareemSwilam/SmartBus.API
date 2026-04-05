using Microsoft.Extensions.DependencyInjection;
using SmartBus.Application.IServices;
using SmartBus.Application.Services;
using SmartBus.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.DependencyInjection
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IComapnyServices, CompanyServices>();
            services.AddScoped<IDriverServices, DriverServices>();
            return services;

        }
    }
}
