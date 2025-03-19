using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DataAccessLayer.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("identity");

            builder.Entity<Url>()
                .HasIndex(u => u.OriginalUrl)
                .IsUnique();

            builder.Entity<Url>()
                .HasOne(u => u.CreatedBy)
                .WithMany(u => u.CreatedUrls)
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
