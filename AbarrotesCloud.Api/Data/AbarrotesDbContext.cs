using System;
using System.Collections.Generic;
using AbarrotesCloud.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AbarrotesCloud.Api.Data;

public partial class AbarrotesDbContext : DbContext
{
    public AbarrotesDbContext()
    {
    }

    public AbarrotesDbContext(DbContextOptions<AbarrotesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abonoscredito> Abonoscreditos { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Clientescredito> Clientescreditos { get; set; }

    public virtual DbSet<Detalleventa> Detalleventas { get; set; }

    public virtual DbSet<Kardex> Kardices { get; set; }

    public virtual DbSet<Listablanca> Listablancas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Suscripcione> Suscripciones { get; set; }

    public virtual DbSet<Tienda> Tiendas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=54322;Database=postgres;Username=postgres;Password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Abonoscredito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("abonoscredito_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Fechaabono).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Abonoscreditos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("abonoscredito_clienteid_fkey");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Abonoscreditos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("abonoscredito_tiendaid_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Abonoscreditos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("abonoscredito_usuarioid_fkey");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Categoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categorias_tiendaid_fkey");
        });

        modelBuilder.Entity<Clientescredito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clientescredito_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Estatus).HasDefaultValueSql("'Activo'::character varying");
            entity.Property(e => e.Limitecredito).HasDefaultValueSql("0.00");
            entity.Property(e => e.Saldoactual).HasDefaultValueSql("0.00");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Clientescreditos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clientescredito_tiendaid_fkey");
        });

        modelBuilder.Entity<Detalleventa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detalleventas_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Producto).WithMany(p => p.Detalleventa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalleventas_productoid_fkey");

            entity.HasOne(d => d.Venta).WithMany(p => p.Detalleventa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalleventas_ventaid_fkey");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kardex_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Fechamovimiento).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Producto).WithMany(p => p.Kardices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_productoid_fkey");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Kardices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_tiendaid_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Kardices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_usuarioid_fkey");
        });

        modelBuilder.Entity<Listablanca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("listablanca_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.FechaAgregado).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Preciocompra).HasDefaultValueSql("0.00");
            entity.Property(e => e.Precioventa).HasDefaultValueSql("0.00");
            entity.Property(e => e.Stockactual).HasDefaultValueSql("0.000");
            entity.Property(e => e.Stockminimo).HasDefaultValueSql("0.000");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productos_categoriaid_fkey");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Productos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productos_tiendaid_fkey");
        });

        modelBuilder.Entity<Suscripcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suscripciones_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Fechapago).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Suscripciones)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("suscripciones_tiendaid_fkey");
        });

        modelBuilder.Entity<Tienda>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tiendas_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Estatus).HasDefaultValueSql("'Activo'::character varying");
            entity.Property(e => e.Estatuslicencia).HasDefaultValueSql("'activo'::character varying");
            entity.Property(e => e.Fecharegistro).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_tiendaid_fkey");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ventas_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Estatus).HasDefaultValueSql("'Completada'::character varying");
            entity.Property(e => e.Fechaventa).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Metodopago).HasDefaultValueSql("'Efectivo'::character varying");

            entity.HasOne(d => d.Clientecredito).WithMany(p => p.Venta).HasConstraintName("ventas_clientecreditoid_fkey");

            entity.HasOne(d => d.Tienda).WithMany(p => p.Venta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_tiendaid_fkey");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Venta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_usuarioid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
