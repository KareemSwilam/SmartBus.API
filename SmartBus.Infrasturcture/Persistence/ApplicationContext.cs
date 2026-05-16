using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Persistence
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> context) :base (context)
        {
            
        }
        public DbSet<Location> Locations { get; set; }   
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripStop> TripStops { get; set; }  
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tickect> Tickects { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<ReservedSeat> ReservedSeats { get; set; }
        public DbSet<StopSegment> StopSegments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }

    }
}
