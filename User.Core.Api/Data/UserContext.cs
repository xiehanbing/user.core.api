﻿using Microsoft.EntityFrameworkCore;
using User.Core.Api.Models;

namespace User.Core.Api.Data
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserProperty>()
                .ToTable("UserProperties")
                .HasKey(u => new { u.Key, u.AppUserId, u.Value });
            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);


            modelBuilder.Entity<UserTag>()
                .ToTable("UserTags")
                .HasKey(u => new { u.UserId, u.Tag });
            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);

            modelBuilder.Entity<BpFile>()
                .ToTable("UserBPFiles")
                .HasKey(b => b.Id);


            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<AppUser> Users { get; set; }

        public DbSet<UserProperty> UserProperties { get; set; }

        public DbSet<UserTag> UserTags { get; set; }
    }
}