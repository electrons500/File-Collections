using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FilesCollections.Models.Data.FilesCollectionsContext
{
    public partial class FilesCollectionsContext : DbContext
    {
        public FilesCollectionsContext()
        {
        }

        public FilesCollectionsContext(DbContextOptions<FilesCollectionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FilesOnDatabase> FilesOnDatabase { get; set; }
        public virtual DbSet<FilesOnFileSystem> FilesOnFileSystem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
// To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilesOnDatabase>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("PK_FileId");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Extension).HasMaxLength(10);

                entity.Property(e => e.FileType).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<FilesOnFileSystem>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Extension).HasMaxLength(10);

                entity.Property(e => e.FilePath).HasMaxLength(100);

                entity.Property(e => e.FileType).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
