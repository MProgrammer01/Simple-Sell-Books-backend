using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleSellBooks_DataLayer.Models;

public partial class SimpleSellBooksDbContext : DbContext
{
    public SimpleSellBooksDbContext()
    {
    }

    public SimpleSellBooksDbContext(DbContextOptions<SimpleSellBooksDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SecurityAuditLog> SecurityAuditLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=SimpleSellBooksDB;User Id=sa;Password=123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SecurityAuditLog>(entity =>
        {
            entity.HasIndex(e => e.EventType, "IX_SecurityAuditLogs_EventType");

            entity.HasIndex(e => e.Timestamp, "IX_SecurityAuditLogs_Timestamp");

            entity.HasIndex(e => e.UserId, "IX_SecurityAuditLogs_UserId");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Endpoint).HasMaxLength(500);
            entity.Property(e => e.EventType).HasMaxLength(100);
            entity.Property(e => e.HttpMethod).HasMaxLength(10);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.TargetId).HasMaxLength(100);
            entity.Property(e => e.TargetType).HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(sysutcdatetime())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
