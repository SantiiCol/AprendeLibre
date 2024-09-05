using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendeLibre.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    [NotMapped]
    public string ConfirmarClave { get; set; } = null!;
    public int RolId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Role Rol { get; set; } = null!;
}
