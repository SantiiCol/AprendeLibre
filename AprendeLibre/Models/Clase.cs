using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Clase
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Materia { get; set; } = null!;

    public string SubirArchivo { get; set; } = null!;

    public string Imagen { get; set; } = null!;

    public int IdGrados { get; set; }

    public virtual Grado IdGradosNavigation { get; set; } = null!;
}
