using Mayhem.Dal.MappingsConfiguration.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.MappingsConfiguration
{
    public class MappingConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameVersionMappingConfiguration());
        }
    }
}
