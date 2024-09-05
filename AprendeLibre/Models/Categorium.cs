using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();

    public virtual ICollection<Libro> LibrosNavigation { get; set; } = new List<Libro>();
}
