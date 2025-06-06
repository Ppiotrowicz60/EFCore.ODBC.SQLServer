﻿using Microsoft.EntityFrameworkCore;

namespace EFCore.ODBC.SQLServer.Tests;

public class LocalContext : DbContext
{
    public DbSet<ComplexEntity> ComplexEntities { get; set; }
    public DbSet<ChildEntity> ChildEntities { get; set; }
    public DbSet<RelatedEntity> RelatedEntities { get; set; }

    public LocalContext(DbContextOptions<LocalContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComplexEntity>(entity =>
        {
            entity.ToTable("ComplexEntity");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StringValue).IsRequired();
            entity.Property(e => e.IntValue);
            entity.Property(e => e.DecimalValue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DateTimeValue).HasColumnType("smalldatetime");
            entity.Property(e => e.BoolValue);
            entity.Property(e => e.GuidValue);
            entity.HasMany(e => e.Children).WithOne().HasForeignKey(c => c.ParentId);
        });

        modelBuilder.Entity<ChildEntity>(entity =>
        {
            entity.ToTable("ChildEntity");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<RelatedEntity>(entity =>
        {
            entity.ToTable("RelatedEntity");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SharedIntValue);
            entity.Property(e => e.Description).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}

public class ComplexEntity
{
    public int Id { get; set; }
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public decimal DecimalValue { get; set; }
    public DateTime DateTimeValue { get; set; }
    public bool BoolValue { get; set; }
    public Guid GuidValue { get; set; }
    public List<ChildEntity> Children { get; set; }
}

public class ChildEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentId { get; set; }
}

public class RelatedEntity
{
    public int Id { get; set; }
    public int SharedIntValue { get; set; }
    public string Description { get; set; }
}