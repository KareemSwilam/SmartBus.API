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
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.Token).IsRequired();
            builder.Property(rt => rt.ExpiryDate).IsRequired();
            builder.Property(rt => rt.IsUsed).HasDefaultValue(false);
            builder.Property(rt => rt.IsRevoked).HasDefaultValue(false);

        }
    }
}
