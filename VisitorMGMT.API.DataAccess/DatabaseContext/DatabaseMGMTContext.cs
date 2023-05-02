using Microsoft.EntityFrameworkCore;
using VisitorMGMT.API.DataAccess.Entities;

namespace VisitorMGMT.API.DataAccess.DatabaseContext
{
    public class DatabaseMGMTContext : DbContext
    {
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DatabaseMGMTContext(DbContextOptions<DatabaseMGMTContext> options) : base(options)
        {

        }
        public DatabaseMGMTContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Visitor>()
            .HasOne(e => e.Profile)
            .WithOne(e => e.Visitor)
            .HasForeignKey<Profile>("VisitorId");
        }
    }
}
