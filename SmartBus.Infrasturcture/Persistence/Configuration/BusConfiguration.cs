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
    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.HasMany(b => b.Seats).WithOne(s => s.Bus)
                .HasForeignKey(s => s.BusId)
                .OnDelete(DeleteBehavior.Cascade);
             builder.HasMany(b => b.Trips)
                .WithOne(t => t.Bus)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
