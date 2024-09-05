using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class BlockNota
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime Fecha { get; set; }
}
