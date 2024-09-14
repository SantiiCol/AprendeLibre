using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AprendeLibre.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DesarrolloAprendeLibre.Controllers
{
    public class LibroController : Controller
    {
        private readonly AplDbContext _context;

        public LibroController(AplDbContext context)
        {
            _context = context;
        }

        public IActionResult LibrosPorCategoria(int id)
        {
            var categoria = _context.Categoria
                .Include(c => c.Libros)
                .FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            var model = new Categorium
            {
                Id = categoria.Id,
                NombreCategoria = categoria.NombreCategoria,
                Libros = categoria.Libros // Carga todos los libros para esta categoría inicialmente
            };

            return View(model); // Carga la vista principal con todos los libros
        }



        public IActionResult FiltrarLibrosPorCategoria(int id, string query)
        {
            var categoria = _context.Categoria
                .Include(c => c.Libros)
                .FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            var libros = categoria.Libros.AsQueryable();

            // Filtrar los libros según el texto ingresado
            if (!string.IsNullOrEmpty(query))
            {
                libros = libros.Where(l => l.NombreLibro.Contains(query) || l.Autor.Contains(query));
            }

            return PartialView("_LibrosCardPartial", libros.ToList()); // Retorna los libros filtrados
        }






        [HttpGet]
        public IActionResult Create(int? idCategoria)
        {
            ViewBag.Categorias = new SelectList(_context.Categoria.ToList(), "Id", "NombreCategoria");

            // Si idCategoria está presente, lo agregamos al ViewBag
            if (idCategoria.HasValue)
            {
                ViewBag.IdCategoria = idCategoria.Value;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Autor,NombreLibro,Descripcion,Imagen,PdfUrl,IdCategoria")] Libro libro, IFormFile? Imagen, IFormFile? PdfUrl)
        {
            // Imprime los errores de ModelState para depuración
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categoria.ToList(), "Id", "NombreCategoria", libro.IdCategoria);
                ViewBag.IdCategoria = libro.IdCategoria; // Pasar el ID de la categoría a ViewBag
                return View(libro);
            }

            if (Imagen != null && Imagen.Length > 0)
            {
                var fileName = Path.GetFileName(Imagen.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }

                libro.Imagen = "/img/" + fileName;
            }

            // Manejo del PDF
            if (PdfUrl != null && PdfUrl.Length > 0)
            {
                var pdfFileName = Path.GetFileName(PdfUrl.FileName);
                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", pdfFileName);

                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    await PdfUrl.CopyToAsync(stream);
                }

                libro.PdfUrl = "/pdfs/" + pdfFileName;
            }

            try
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction("LibrosPorCategoria", "Libro", new { id = libro.IdCategoria });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar en la base de datos: " + ex.Message);
                ViewBag.Categorias = new SelectList(_context.Categoria.ToList(), "Id", "NombreCategoria", libro.IdCategoria);
                ViewBag.IdCategoria = libro.IdCategoria; // Pasar el ID de la categoría a ViewBag
                return View(libro);
            }
        }









        [HttpPost]
        public IActionResult GetCategorias()
        {
            var categorias = _context.Categoria.ToList();
            foreach (var categoria in categorias)
            {
                Console.WriteLine($"ID: {categoria.Id}, Nombre: {categoria.NombreCategoria}");
            }
            return Json(categorias);
        }




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = _context.Categoria.ToList();
            return View(libro);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro, IFormFile? Imagen, IFormFile? PDF)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            // Cargar el libro existente desde la base de datos
            var libroExistente = await _context.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
            if (libroExistente == null)
            {
                return NotFound();
            }

            // Verificación de errores de validación
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = _context.Categoria.ToList();
                return View(libro);
            }

            try
            {
                // Manejar la imagen: Si no hay nueva imagen, mantener la existente
                if (Imagen != null && Imagen.Length > 0)
                {
                    var fileName = Path.GetFileName(Imagen.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }

                    libro.Imagen = "/img/" + fileName;
                }
                else
                {
                    libro.Imagen = libroExistente.Imagen; // Mantener la imagen existente
                }

                // Manejar el PDF: Si no hay nuevo PDF, mantener el existente
                if (PDF != null && PDF.Length > 0)
                {
                    var pdfFileName = Path.GetFileName(PDF.FileName);
                    var pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", pdfFileName);

                    using (var pdfStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        await PDF.CopyToAsync(pdfStream);
                    }

                    libro.PdfUrl = "/pdfs/" + pdfFileName;
                }
                else
                {
                    libro.PdfUrl = libroExistente.PdfUrl; // Mantener el PDF existente
                }

                // Actualizar el resto de los campos
                _context.Attach(libro);
                _context.Entry(libro).Property(x => x.Imagen).IsModified = Imagen != null && Imagen.Length > 0;
                _context.Entry(libro).Property(x => x.PdfUrl).IsModified = PDF != null && PDF.Length > 0;
                _context.Entry(libro).Property(x => x.NombreLibro).IsModified = true;
                _context.Entry(libro).Property(x => x.Autor).IsModified = true;
                _context.Entry(libro).Property(x => x.Descripcion).IsModified = true;
                _context.Entry(libro).Property(x => x.IdCategoria).IsModified = true;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(libro.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el libro: " + ex.Message);
                ViewBag.Categorias = _context.Categoria.ToList();
                return View(libro);
            }

            return RedirectToAction(nameof(LibrosPorCategoria), new { id = libro.IdCategoria });
        }







        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro); // Se pasa el modelo "Libro" a la vista
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(LibrosPorCategoria), new { id = libro.IdCategoria });
        }








        public async Task<IActionResult> ConfirmarEliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro); // Retorna una vista de confirmación antes de eliminar
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            // Verificar si hay valores nulos en las propiedades del libro
            // y proporcionar valores predeterminados si es necesario.
            libro.Autor = libro.Autor ?? "Autor no disponible";
            libro.NombreLibro = libro.NombreLibro ?? "Nombre no disponible";
            libro.Descripcion = libro.Descripcion ?? "Descripción no disponible";
            libro.Imagen = libro.Imagen ?? "Imagen no disponible";
            libro.PdfUrl = libro.PdfUrl ?? "URL de PDF no disponible";

            return PartialView("_DetallesLibroPartial", libro);
        }




        public async Task<IActionResult> Leer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }
    }
}

