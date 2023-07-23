using DB.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace DB.Data
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        { 
        }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable(nameof(Client));

            base.OnModelCreating(modelBuilder);
        }
    }
}
