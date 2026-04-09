using Microsoft.Extensions.DependencyInjection;
using SmartBus.Application.IServices;
using SmartBus.Domain.IRepository;
using SmartBus.Infrasturcture.ExternalServicesImplementation.LocationExternalServices;
using SmartBus.Infrasturcture.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.DependencyInjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IBookingRepository, BookingRepository>();
                services.AddScoped<IBusRepository, BusRepository>();
                services.AddScoped<ICompanyRepository, CompanyRepository>();
                services.AddScoped<IDervierRepository, DervierRepository>();
                services.AddScoped<ILocationRepository, LocationRepository>();
                services.AddScoped<ISeatRepository, SeatRepository>();
                services.AddScoped<ITicketRepository, TicketRepository>();
                services.AddScoped<ITripRepository, TripRepository>();
                services.AddScoped<ITripStopRepository, TripStopRepository>();
                services.AddScoped<ILocationServices, LocationServices>();
            return services;
        }

    }
}
