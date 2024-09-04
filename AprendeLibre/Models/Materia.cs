using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Materia
{
    public int Id { get; set; }

    public string NombreMateria { get; set; } = null!;

    public virtual ICollection<Grado> Grados { get; set; } = new List<Grado>();
}
