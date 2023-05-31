using CookieReader.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CookieReader
{
    public class CookieDbCtx : DbContext
    {
        public DbSet<Cookies>? Cookies { get; set; }
        public DbSet<Meta>? Meta { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                Mode = SqliteOpenMode.ReadOnly,
                DataSource = Consts.CookieDbPath,
            };
            _ = optionsBuilder.UseSqlite(new SqliteConnection(connectionStringBuilder.ToString()));
        }
    }
}
