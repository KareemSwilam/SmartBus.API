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
    public class DeriverConfiiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.HasMany(d => d.Trips).WithOne(t => t.Dervier)
                   .HasForeignKey(t => t.DervierId)
                   .OnDelete(DeleteBehavior.Cascade);   
            builder.HasMany(d => d.Reviews).WithOne()
                   .HasForeignKey(r => r.TargetId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
