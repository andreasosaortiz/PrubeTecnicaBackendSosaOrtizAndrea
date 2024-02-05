using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaBackendconEntityFramework.Models;

public partial class DbpruebatecnicabackendContext : DbContext
{
    public DbpruebatecnicabackendContext()
    {
    }

    public DbpruebatecnicabackendContext(DbContextOptions<DbpruebatecnicabackendContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auto> Autos { get; set; }

    public virtual DbSet<Estancium> Estancia { get; set; }

    public virtual DbSet<Tipo> Tipos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
  //      => optionsBuilder.UseSqlServer("server=desktop-7g3tfjf\\sqlexpress; database=DBPRUEBATECNICABACKEND; integrated security=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auto>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__AUTO__3214EC27D11BF13E");

            entity.ToTable("AUTO");

            entity.HasIndex(e => e.Patente, "UQ__AUTO__CA655166F670934E").IsUnique();

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.Patente)
                .HasMaxLength(7)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Estancium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTANCIA__3214EC27C17897E9");

            entity.ToTable("ESTANCIA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HsEntrada).HasColumnType("datetime");
            entity.Property(e => e.HsSalida).HasColumnType("datetime");
            entity.Property(e => e.IdAuto)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAutoNavigation).WithMany(p => p.Estancia)
                .HasPrincipalKey(p => p.Patente)
                .HasForeignKey(d => d.IdAuto)
                .HasConstraintName("FK_IdAuto");
        });

        modelBuilder.Entity<Tipo>(entity =>
        {
            entity.HasKey(e => e.Idtipo).HasName("PK__TIPO__BEB088A6E124945A");

            entity.ToTable("TIPO");

            entity.Property(e => e.Idtipo).HasColumnName("IDTipo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USUARIOS__3214EC2779A36B00");

            entity.ToTable("USUARIOS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdAuto)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Idtipo).HasColumnName("IDTipo");

            entity.HasOne(d => d.IdAutoNavigation).WithMany(p => p.Usuarios)
                .HasPrincipalKey(p => p.Patente)
                .HasForeignKey(d => d.IdAuto)
                .HasConstraintName("FK_AutoID");

            entity.HasOne(d => d.IdtipoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Idtipo)
                .HasConstraintName("FK_IDTipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
