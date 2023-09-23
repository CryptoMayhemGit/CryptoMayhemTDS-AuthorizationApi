using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mayhem.Dal.MappingsConfiguration.Configurations
{
    public class GameVersionMappingConfiguration : IEntityTypeConfiguration<GameUser>
    {
        public void Configure(EntityTypeBuilder<GameUser> builder)
        {
            builder.ToTable(nameof(GameUser));

            builder.Property(e => e.Id);
            builder.Property(e => e.Wallet).HasMaxLength(50).IsRequired();
        }
    }
}
