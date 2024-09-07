using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MealProject.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Likedrecipe> Likedrecipes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Userrecipe> Userrecipes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521) (CONNECT_DATA=(SID=xe))));User Id=C##MealProject; Password=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##MEALPROJECT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Contactid).HasName("SYS_C009160");

            entity.ToTable("CONTACT");

            entity.Property(e => e.Contactid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTID");
            entity.Property(e => e.Contant)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTANT");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C009148");

            entity.ToTable("CUSTOMER");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Googleid)
                .HasColumnType("NUMBER")
                .HasColumnName("GOOGLEID");
            entity.Property(e => e.Registerdate)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n")
                .HasColumnName("REGISTERDATE");
            entity.Property(e => e.Useremail)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("USEREMAIL");
            entity.Property(e => e.Userimage)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("USERIMAGE");
            entity.Property(e => e.Username)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Userpassword)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("USERPASSWORD");
        });

        modelBuilder.Entity<Likedrecipe>(entity =>
        {
            entity.HasKey(e => e.Likedid).HasName("SYS_C009154");

            entity.ToTable("LIKEDRECIPE");

            entity.Property(e => e.Likedid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("LIKEDID");
            entity.Property(e => e.Createdat)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATEDAT");
            entity.Property(e => e.Recipedata)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("RECIPEDATA");
            entity.Property(e => e.Recipeimage)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("RECIPEIMAGE");
            entity.Property(e => e.Recipelabel)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("RECIPELABEL");
            entity.Property(e => e.Userloginid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGINID");

            entity.HasOne(d => d.Userlogin).WithMany(p => p.Likedrecipes)
                .HasForeignKey(d => d.Userloginid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_LIKED_RECIPES_USERLOGIN");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C009146");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Userloginid).HasName("SYS_C009150");

            entity.ToTable("USERLOGIN");

            entity.Property(e => e.Userloginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGINID");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ROLE_USERLOGIN");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CUSTOMER_USERLOGIN");
        });

        modelBuilder.Entity<Userrecipe>(entity =>
        {
            entity.HasKey(e => e.Recipesid).HasName("SYS_C009157");

            entity.ToTable("USERRECIPES");

            entity.Property(e => e.Recipesid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPESID");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Ingredients)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("INGREDIENTS");
            entity.Property(e => e.Title)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.Userloginid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGINID");
            entity.Property(e => e.Video)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("VIDEO");

            entity.HasOne(d => d.Userlogin).WithMany(p => p.Userrecipes)
                .HasForeignKey(d => d.Userloginid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_USERRECIPES_USERLOGIN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
