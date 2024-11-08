﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Milestone2.FirstApplication.Models;

public partial class Milestone2DBContext : DbContext
{
    public Milestone2DBContext()
    {
    }

    public Milestone2DBContext(DbContextOptions<Milestone2DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarOwnership> CarOwnerships { get; set; }

    public virtual DbSet<Make> Makes { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=tcp:s30.winhost.com;Initial Catalog=DB_128040_rai0006;Persist Security Info=True;User ID=DB_128040_rai0006_user;Password=Roshan113487124; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Vin).HasName("PK__Car__C5DF234D01B2EF50");

            entity.ToTable("Car");

            entity.Property(e => e.Vin)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VIN");
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.MakeId).HasColumnName("MakeID");
            entity.Property(e => e.Model)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Year)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.HasOne(d => d.Make).WithMany(p => p.Cars)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__MakeID__3F466844");
        });

        modelBuilder.Entity<CarOwnership>(entity =>
        {
            entity.HasKey(e => e.OwnershipId).HasName("PK__CarOwner__A0D871E97C8A8698");

            entity.ToTable("CarOwnership");

            entity.Property(e => e.OwnershipId).HasColumnName("OwnershipID");
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.PurchaseDate).HasColumnType("date");
            entity.Property(e => e.SaleDate).HasColumnType("date");
            entity.Property(e => e.Vin)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VIN");

            entity.HasOne(d => d.Owner).WithMany(p => p.CarOwnerships)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwners__Owner__4222D4EF");

            entity.HasOne(d => d.VinNavigation).WithMany(p => p.CarOwnerships)
                .HasForeignKey(d => d.Vin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarOwnershi__VIN__4316F928");
        });

        modelBuilder.Entity<Make>(entity =>
        {
            entity.HasKey(e => e.MakeId).HasName("PK__Make__43646F314C155C50");

            entity.ToTable("Make");

            entity.Property(e => e.MakeId).HasColumnName("MakeID");
            entity.Property(e => e.BrandName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ceoname)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CEOName");
            entity.Property(e => e.FoundingYear)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.WebsiteLink)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owner__81938598D662029A");

            entity.ToTable("Owner");

            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.OwnerName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}