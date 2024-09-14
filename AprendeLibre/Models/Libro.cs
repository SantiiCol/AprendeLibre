using System;
using System.Collections.Generic;

namespace AprendeLibre.Models;

public partial class Libro
{
    public int Id { get; set; }

    public string Autor { get; set; } = null!;

    public string NombreLibro { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? Imagen { get; set; }

    public string? PdfUrl { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual ICollection<Categorium> Categoria { get; set; } = new List<Categorium>();
}
