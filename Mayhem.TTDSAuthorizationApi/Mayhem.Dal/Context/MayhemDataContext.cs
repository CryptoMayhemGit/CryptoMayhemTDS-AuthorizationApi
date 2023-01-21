using Mayhem.Dal.MappingsConfiguration;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;

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
        public virtual DbSet<GameUserBlock> GameUserBlocks { get; set; }
        public virtual DbSet<VoteCategory> VoteCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingConfiguration.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }
    }
}
