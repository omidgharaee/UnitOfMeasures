using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfMeasures.Core.Domain.Dimension.Entities;

namespace UnitOfMeasures.Infrastructure.Persistence.Configs
{
    internal class DimensionConfiguration : IEntityTypeConfiguration<UnitOfMeasures.Core.Domain.Dimension.Entities.Dimension>
    {
        public void Configure(EntityTypeBuilder<UnitOfMeasures.Core.Domain.Dimension.Entities.Dimension> builder)
        {
            builder.HasMany(u => u.Units).WithOne(u => u.Dimension).HasForeignKey(u => u.DimensionId);
        }
    }
}
