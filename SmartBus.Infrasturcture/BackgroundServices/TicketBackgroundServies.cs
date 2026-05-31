using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using SmartBus.Application.IServices;
using SmartBus.Application.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.BackgroundServices
{
    public class TicketBackgroundServies : BackgroundService
    {
        private readonly IServiceProvider _servicesProvider;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public TicketBackgroundServies(IServiceProvider servicesProvider, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _servicesProvider = servicesProvider;
            _backgroundTaskQueue = backgroundTaskQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var bookingId =
                        await _backgroundTaskQueue.DequeueAsync(stoppingToken);

                    using var scope = _servicesProvider.CreateScope();

                    var ticketService =
                        scope.ServiceProvider.GetRequiredService<ITicketServices>();

                    Console.WriteLine($"Processing booking ID: {bookingId}");

                    await ticketService.GenerateAndSendTicketAsync(bookingId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Background job failed: {ex}");
                }
            } 
        }
    }

}
