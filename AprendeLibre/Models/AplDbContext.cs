using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AprendeLibre.Models;

public partial class AplDbContext : DbContext
{
    public AplDbContext()
    {
    }

    public AplDbContext(DbContextOptions<AplDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlockNota> BlockNotas { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Clase> Clases { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Grado> Grados { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-TDP2QL3\\SQLEXPRESS; Database=APL_DB; Integrated Security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlockNota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlockNot__3213E83FF58F31BD");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(255);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83F9B8F8547");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NombreCategoria).HasMaxLength(255);
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clases__3213E83F8A275EE4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.IdGrados).HasColumnName("Id_Grados");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Materia).HasMaxLength(255);
            entity.Property(e => e.SubirArchivo).HasMaxLength(255);
            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.IdGradosNavigation).WithMany(p => p.Clases)
                .HasForeignKey(d => d.IdGrados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clases_Grados");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comentar__3213E83F7FBEC386");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comentario1)
                .HasMaxLength(255)
                .HasColumnName("Comentario");
            entity.Property(e => e.IdGrados).HasColumnName("Id_Grados");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            entity.HasOne(d => d.IdGradosNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdGrados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentarios_Grados");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentarios_Usuarios");
        });

        modelBuilder.Entity<Grado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grados__3213E83FE53D0E1A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NombreGrado).HasMaxLength(255);

            entity.HasMany(d => d.Materias).WithMany(p => p.Grados)
                .UsingEntity<Dictionary<string, object>>(
                    "GradosMateria",
                    r => r.HasOne<Materia>().WithMany()
                        .HasForeignKey("MateriasId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Grados_Materias_Materias"),
                    l => l.HasOne<Grado>().WithMany()
                        .HasForeignKey("GradosId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Grados_Materias_Grados"),
                    j =>
                    {
                        j.HasKey("GradosId", "MateriasId").HasName("PK__Grados_M__B36A28B88FD67482");
                        j.ToTable("Grados_Materias");
                        j.IndexerProperty<int>("GradosId").HasColumnName("Grados_Id");
                        j.IndexerProperty<int>("MateriasId").HasColumnName("Materias_id");
                    });
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Libros__3213E83FFE70F228");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Autor).HasMaxLength(255);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.NombreLibro).HasMaxLength(255);
            entity.Property(e => e.PdfUrl).HasMaxLength(255);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Libros_Categoria");

            entity.HasMany(d => d.Categoria).WithMany(p => p.LibrosNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "LibrosCategorium",
                    r => r.HasOne<Categorium>().WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Libros_Categoria_Categoria"),
                    l => l.HasOne<Libro>().WithMany()
                        .HasForeignKey("LibrosId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Libros_Categoria_Libros"),
                    j =>
                    {
                        j.HasKey("LibrosId", "CategoriaId").HasName("PK__Libros_C__90CE5D48205B2977");
                        j.ToTable("Libros_Categoria");
                        j.IndexerProperty<int>("LibrosId").HasColumnName("Libros_Id");
                        j.IndexerProperty<int>("CategoriaId").HasColumnName("Categoria_id");
                    });
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Materias__3213E83FDAAE9F50");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NombreMateria).HasMaxLength(255);
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recursos__3213E83FDBB09EF1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Archivo).HasMaxLength(255);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Materia).HasMaxLength(255);
            entity.Property(e => e.Titulo).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83F05CF4266");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol).HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__771110D532B95B4A");

            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_Id");
            entity.Property(e => e.Apellido).HasMaxLength(255);
            entity.Property(e => e.Clave).HasMaxLength(255);
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.NombreUsuario).HasMaxLength(255);
            entity.Property(e => e.Nombres).HasMaxLength(255);
            entity.Property(e => e.RolId).HasColumnName("Rol_Id");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
