using Mayhem.Dal.MappingsConfiguration;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.Context
{
    public class MayhemDataContext : DbContext
    {
        public MayhemDataContext()
        {
        }

        public MayhemDataContext(DbContextOptions<MayhemDataContext> options) : base(options)
        {
        }

        public virtual DbSet<GameUser> GameUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingConfiguration.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
