using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Recurso
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Materia { get; set; } = null!;

    public string Imagen { get; set; } = null!;

    public string Archivo { get; set; } = null!;
}
