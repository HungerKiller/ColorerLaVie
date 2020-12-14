using Microsoft.EntityFrameworkCore;
using PhotoMasterBackend.Models;

namespace PhotoMasterBackend
{
    public class PhotoContext : DbContext
    {
        public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Label> Labels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>().ToTable("Photos");
            modelBuilder.Entity<Label>().ToTable("Labels");
        }
    }
}
