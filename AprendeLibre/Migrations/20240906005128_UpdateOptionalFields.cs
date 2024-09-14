using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AprendeLibre.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOptionalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockNotas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BlockNot__3213E83FF58F31BD", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__3213E83F9B8F8547", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Grados",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreGrado = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Grados__3213E83FE53D0E1A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMateria = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Materias__3213E83FDAAE9F50", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Materia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Archivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Recursos__3213E83FDBB09EF1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__3213E83F05CF4266", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Autor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NombreLibro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PdfUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Libros__3213E83FFE70F228", x => x.id);
                    table.ForeignKey(
                        name: "FK_Libros_Categoria",
                        column: x => x.Id_Categoria,
                        principalTable: "Categoria",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Materia = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubirArchivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Id_Grados = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clases__3213E83F8A275EE4", x => x.id);
                    table.ForeignKey(
                        name: "FK_Clases_Grados",
                        column: x => x.Id_Grados,
                        principalTable: "Grados",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Grados_Materias",
                columns: table => new
                {
                    Grados_Id = table.Column<int>(type: "int", nullable: false),
                    Materias_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Grados_M__B36A28B88FD67482", x => new { x.Grados_Id, x.Materias_id });
                    table.ForeignKey(
                        name: "FK_Grados_Materias_Grados",
                        column: x => x.Grados_Id,
                        principalTable: "Grados",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Grados_Materias_Materias",
                        column: x => x.Materias_id,
                        principalTable: "Materias",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rol_Id = table.Column<int>(type: "int", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__771110D532B95B4A", x => x.Usuario_Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles",
                        column: x => x.Rol_Id,
                        principalTable: "Roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Libros_Categoria",
                columns: table => new
                {
                    Libros_Id = table.Column<int>(type: "int", nullable: false),
                    Categoria_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Libros_C__90CE5D48205B2977", x => new { x.Libros_Id, x.Categoria_id });
                    table.ForeignKey(
                        name: "FK_Libros_Categoria_Categoria",
                        column: x => x.Categoria_id,
                        principalTable: "Categoria",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Libros_Categoria_Libros",
                        column: x => x.Libros_Id,
                        principalTable: "Libros",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comentario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Id_Grados = table.Column<int>(type: "int", nullable: false),
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comentar__3213E83F7FBEC386", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Grados",
                        column: x => x.Id_Grados,
                        principalTable: "Grados",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Usuario_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clases_Id_Grados",
                table: "Clases",
                column: "Id_Grados");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_Id_Grados",
                table: "Comentarios",
                column: "Id_Grados");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_Id_Usuario",
                table: "Comentarios",
                column: "Id_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Grados_Materias_Materias_id",
                table: "Grados_Materias",
                column: "Materias_id");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Id_Categoria",
                table: "Libros",
                column: "Id_Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Categoria_Categoria_id",
                table: "Libros_Categoria",
                column: "Categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Rol_Id",
                table: "Usuarios",
                column: "Rol_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockNotas");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Grados_Materias");

            migrationBuilder.DropTable(
                name: "Libros_Categoria");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Grados");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
