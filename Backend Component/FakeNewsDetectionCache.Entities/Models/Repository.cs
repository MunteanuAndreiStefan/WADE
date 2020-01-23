using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FakeNewsDetectionCache.Entities.Models
{
    public partial class Repository : DbContext
    {
        public Repository()
        {
        }

        public Repository(DbContextOptions<Repository> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<NewsArticle> NewsArticles { get; set; }
        public virtual DbSet<TwitterUser> TwitterUsers { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(e => e.UserId).IsUnique();

                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_FK_NewsArticeUser");

                entity.HasIndex(e => e.Source).IsUnique();

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NewsArticles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsArticeUser");
            });

            modelBuilder.Entity<TwitterUser>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasIndex(e => e.ApplicationUserId)
                    .HasName("IX_FK_VoteApplicationUser");

                entity.HasIndex(e => e.NewsArticleId)
                    .HasName("IX_FK_VoteNewsArticle");

                entity.Property(e => e.ApplicationUserId).HasColumnName("ApplicationUser_Id");

                entity.Property(e => e.NewsArticleId).HasColumnName("NewsArticle_Id");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoteApplicationUser");

                entity.HasOne(d => d.NewsArticle)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(d => d.NewsArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoteNewsArticle");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
