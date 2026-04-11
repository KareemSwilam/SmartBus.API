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
    public class TripStopConfiguration : IEntityTypeConfiguration<TripStop>
    {
        public void Configure(EntityTypeBuilder<TripStop> builder)
        {
            

            builder.HasOne(ts => ts.Location)
                   .WithMany()
                   .HasForeignKey(ts => ts.LocationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
