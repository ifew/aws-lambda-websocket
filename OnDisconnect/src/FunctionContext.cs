using Microsoft.EntityFrameworkCore;

namespace OnDisconnect
{
    public class FunctionContext : DbContext
    {
        public FunctionContext(DbContextOptions<FunctionContext> dbContextOptions) : base(dbContextOptions){ }

        public DbSet<ConnectionSocketModel> Connections { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}