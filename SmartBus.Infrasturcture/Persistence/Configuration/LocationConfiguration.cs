using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Persistence.Configuration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasMany(l => l.StartTripsHere)
                   .WithOne(t => t.StartLocation)
                   .HasForeignKey(t => t.StartLocationId)
                   .OnDelete(DeleteBehavior.Restrict); 
            builder.HasMany(l => l.EndTripsHere)
                   .WithOne(t => t.EndLocation)
                   .HasForeignKey(t => t.EndLocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
