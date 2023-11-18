using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace amazonCloneWebAPI.Models;

public partial class AmazonCloneContext : DbContext
{
    public AmazonCloneContext()
    {
    }

    public AmazonCloneContext(DbContextOptions<AmazonCloneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6EDFBA9E4C2");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TknRfrshTokenId).HasName("PK__RefreshT__5D3C7AFC5EAE93F3");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.TknRfrshTokenId).HasColumnName("tkn_RfrshTokenID");
            entity.Property(e => e.TknIsActive).HasColumnName("tkn_IsActive");
            entity.Property(e => e.TknRfrshToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tkn_RfrshToken");
            entity.Property(e => e.TknTokenId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tkn_TokenID");
            entity.Property(e => e.TknUserId).HasColumnName("tkn_UserID");

            entity.HasOne(d => d.TknUser).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.TknUserId)
                .HasConstraintName("FK__RefreshTo__tkn_U__5AEE82B9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleRoleId).HasName("PK__Roles__343DE54F6F854F5A");

            entity.Property(e => e.RoleRoleId).HasColumnName("role_RoleID");
            entity.Property(e => e.RoleRoleName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("role_RoleName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsrUserId).HasName("PK__Users__3C6D71F1B201DE4E");

            entity.Property(e => e.UsrUserId).HasColumnName("usr_UserId");
            entity.Property(e => e.UsrAge).HasColumnName("usr_Age");
            entity.Property(e => e.UsrCountry)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usr_Country");
            entity.Property(e => e.UsrDateOfBirth)
                .HasColumnType("date")
                .HasColumnName("usr_DateOfBirth");
            entity.Property(e => e.UsrEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usr_Email");
            entity.Property(e => e.UsrFirstName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("usr_FirstName");
            entity.Property(e => e.UsrGender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("usr_Gender");
            entity.Property(e => e.UsrLastName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("usr_LastName");
            entity.Property(e => e.UsrPassword)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("usr_Password");
            entity.Property(e => e.UsrPhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("usr_PhoneNumber");
            entity.Property(e => e.UsrRoleId).HasColumnName("usr_RoleID");
            entity.Property(e => e.UsrUserName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("usr_UserName");

            entity.HasOne(d => d.UsrRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UsrRoleId)
                .HasConstraintName("FK__Users__usr_RoleI__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
