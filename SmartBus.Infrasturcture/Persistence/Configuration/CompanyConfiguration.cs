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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasMany(c => c.Buses).WithOne(b => b.Company)
                   .HasForeignKey(b => b.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Derviers).WithOne(d => d.Company)
                   .HasForeignKey(d => d.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Trips).WithOne(t => t.Company)
                   .HasForeignKey(t => t.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.Reviews).WithOne()
                   .HasForeignKey(r => r.TargetId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
