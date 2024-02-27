using Microsoft.EntityFrameworkCore;
using TravelOrganization2.Model.Entities;

namespace TravelOrganization2.Model.Context
{
    public class DataBaseContext:DbContext
    {
        public DataBaseContext(DbContextOptions options):base(options) {}

        public DbSet<User> users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsRemoved);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<UserToken>().HasKey(p => p.TokenId);
        }
    }
}
