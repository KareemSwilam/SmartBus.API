using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBus.Domain.Models;

namespace SmartBus.Infrasturcture.Persistence.Configuration
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {

            builder.HasMany(t => t.Bookings).WithOne(b => b.Trip)
                   .HasForeignKey(b => b.TripId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.Tickects).WithOne()
                   .HasForeignKey(ti => ti.TripId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.TripStops).WithOne(ts => ts.Trip)
                   .HasForeignKey(ts => ts.TripId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.Reviews).WithOne()
                   .HasForeignKey(r => r.TargetId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.ReservedSeats)
                   .WithOne(rs => rs.Trip).HasForeignKey(rs => rs.TripId);

        }
    }
}
