using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using upband.Data.Entities;

namespace upband.Data
{
    public class DataBaseContext : IdentityDbContext
    {
        public DataBaseContext(DbContextOptions options)  : base(options)
        {
            //Database.EnsureCreated();
            //Database.EnsureDeleted();
        }
        public new void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<user>()
                .HasOne(u => u.ArtistProfile);

            modelBuilder.Entity<user>()
                .HasOne(u => u.UserProfile);
        }
        public DbSet<user> users { get; set; }
        public DbSet<artist> artists { get; set; }
        public DbSet<song> songs { get; set; }
        public DbSet<playlist> playlists { get; set; }
        public DbSet<profile> profiles { get; set; }
    }
}
