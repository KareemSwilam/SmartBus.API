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
    public class RefundRequestConfiguration : IEntityTypeConfiguration<RefundRequest>
    {
        public void Configure(EntityTypeBuilder<RefundRequest> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.UserId)
                   .IsRequired();
            builder.Property(r => r.BookingId)
                   .IsRequired();
            builder.Property(r => r.RequestTime)
                   .IsRequired();
            builder.Property(r => r.Status)
                   .IsRequired();
          
            builder.HasOne(r => r.Booking)
                   .WithOne()
                   .HasForeignKey<RefundRequest>(r => r.BookingId);   
        }
    }
}
