using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartBus.Application.IExternalServices;
using SmartBus.Application.IServices;
using SmartBus.Domain.IRepository;
using SmartBus.Infrasturcture.ExternalServices.MailExternalServices;

using SmartBus.Infrasturcture.Repository;

namespace SmartBus.Infrasturcture.DependencyInjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
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
            services.AddScoped<IReservedSeatRepository, ReservedSeatRepository>();
            services.AddScoped<IStopSegmentRepository,StopSegmentRepository>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddOptions<MailkitSetting>().Bind(configuration.GetSection(MailkitSetting.Name));
            services.AddScoped<ISendingEmailService,SendingEmailService>(); 
            return services;
        }

    }
}
