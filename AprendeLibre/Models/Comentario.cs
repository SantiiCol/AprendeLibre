using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Comentario
{
    public int Id { get; set; }

    public string Comentario1 { get; set; } = null!;

    public int IdGrados { get; set; }

    public int IdUsuario { get; set; }

    public virtual Grado IdGradosNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
