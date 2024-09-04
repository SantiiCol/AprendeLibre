using AprendeLibre.Models;
using AprendeLibre.Permisos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Claims;

namespace AprendeLibre.Controllers
{
    [ValidarSesion]
    public class ClaseController : Controller
    {
        private readonly AplDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ClaseController(AplDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var clases = await _context.Clases.ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Matematicas()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Matematicas").ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Español()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Español").ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Ciencias_Naturales()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Ciencias Naturales").ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Ingles()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Ingles").ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Fisica()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Fisica").ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Sociales()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Sociales").ToListAsync();
            return View(clases);
        }

        public async Task<IActionResult> Etica()
        {
            var clases = await _context.Clases.Where(c => c.Materia == "Etica").ToListAsync();
            return View(clases);
        }


        [Authorize(Policy = "ProfesorPolicy")]
        [HttpGet]
        public IActionResult create()
        {
            return View();
        }

        [Authorize(Policy = "ProfesorPolicy")]
        [HttpPost]
        public async Task<IActionResult> create(Clase clase, IFormFile imagen, IFormFile Archivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (imagen != null && imagen.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(fileStream);
                        }

                        clase.Imagen = "/img/" + uniqueFileName;
                    }

                    // Handle document upload
                    if (Archivo != null && Archivo.Length > 0)
                    {
                        var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                        var fileExtension = Path.GetExtension(Archivo.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("", "Solo se permiten archivos PDF o Word.");
                            return View(clase);
                        }

                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "documents");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Archivo.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Archivo.CopyToAsync(fileStream);
                        }

                        clase.SubirArchivo = "/documents/" + uniqueFileName;
                    }

                    _context.Add(clase);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(clase.Materia);
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "No se pudo guardar los cambios. Inténtalo de nuevo, y si el problema persiste, consulte con su administrador del sistema.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                }
            }
            return View(clase);
        }

        public async Task<IActionResult> Details(int id)
        {
            var clase = await _context.Clases.FirstOrDefaultAsync(m => m.Id == id);
            if (clase == null)
            {
                return NotFound();
            }
            return View(clase);
        }

        [Authorize(Policy = "ProfesorPolicy")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase == null)
            {
                return NotFound();
            }
            return View(clase);
        }

        [Authorize(Policy = "ProfesorPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Clase clase, IFormFile imagen, IFormFile archivo)
        {
            if (id != clase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image update
                    if (imagen != null && imagen.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(fileStream);
                        }

                        clase.Imagen = "/img/" + uniqueFileName;
                    }

                    // Handle document update
                    if (archivo != null && archivo.Length > 0)
                    {
                        var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                        var fileExtension = Path.GetExtension(archivo.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("", "Solo se permiten archivos PDF o Word.");
                            return View(clase);
                        }

                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "documents");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + archivo.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await archivo.CopyToAsync(fileStream);
                        }

                        clase.SubirArchivo = "/documents/" + uniqueFileName;
                    }

                    _context.Update(clase);
                    await _context.SaveChangesAsync();

                    // Redirect to the appropriate subject view
                    return RedirectToAction(clase.Materia);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(clase.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(clase);
        }

        private bool ClassExists(int id)
        {
            return _context.Clases.Any(e => e.Id == id);
        }

        [Authorize(Policy = "ProfesorPolicy")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase == null)
            {
                return NotFound();
            }

            _context.Clases.Remove(clase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaseExists(int id)
        {
            return _context.Clases.Any(e => e.Id== id);
        }

    }
}
