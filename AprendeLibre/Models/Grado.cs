using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Grado
{
    public int Id { get; set; }

    public string NombreGrado { get; set; } = null!;

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Materia> Materias { get; set; } = new List<Materia>();
}
