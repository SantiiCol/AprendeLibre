using AprendeLibre.Models;
using AprendeLibre.Permisos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Index Method
        public async Task<IActionResult> Index()
        {
            var clases = await _context.Clases.ToListAsync();
            return View(clases);
        }

        // Methods for viewing classes by subject
        private async Task<IActionResult> ViewBySubject(string subject)
        {
            var clases = await _context.Clases.Where(c => c.Materia == subject).ToListAsync();
            return View(subject, clases);
        }

        public Task<IActionResult> Matematicas() => ViewBySubject("Matematicas");
        public Task<IActionResult> Español() => ViewBySubject("Español");
        public Task<IActionResult> Ciencias_Naturales() => ViewBySubject("Ciencias Naturales");
        public Task<IActionResult> Ingles() => ViewBySubject("Ingles");
        public Task<IActionResult> Fisica() => ViewBySubject("Fisica");
        public Task<IActionResult> Sociales() => ViewBySubject("Sociales");
        public Task<IActionResult> Etica() => ViewBySubject("Etica");

        // Details Method
        public async Task<IActionResult> Details(int id)
        {
            var clase = await _context.Clases.FirstOrDefaultAsync(m => m.Id == id);
            if (clase == null)
            {
                return NotFound();
            }
            return View(clase);
        }

        // Create Methods
        
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.GradosList = new SelectList(_context.Grados, "Id", "NombreGrado");
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(Clase clase, IFormFile imagen, IFormFile archivo)
        {
            if (!ModelState.IsValid) return View(clase);

            try
            {
                clase.Imagen = await SaveFileAsync(imagen, "img");
                clase.SubirArchivo = await SaveFileAsync(archivo, "documents", new[] { ".pdf", ".doc", ".docx" });

                _context.Add(clase);
                await _context.SaveChangesAsync();

                return RedirectToAction(clase.Materia);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudo guardar los cambios. Inténtalo de nuevo, y si el problema persiste, consulte con su administrador del sistema.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
            }

            return View(clase);
        }

        // Edit Methods
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase == null) return NotFound();

            return View(clase);
        }

        [Authorize(Policy = "AdministradorPolicy")]
        [Authorize(Policy = "ProfesorPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Clase clase, IFormFile imagen, IFormFile archivo)
        {
            if (id != clase.Id) return NotFound();

            if (!ModelState.IsValid) return View(clase);

            try
            {
                clase.Imagen = await SaveFileAsync(imagen, "img");
                clase.SubirArchivo = await SaveFileAsync(archivo, "documents", new[] { ".pdf", ".doc", ".docx" });

                _context.Update(clase);
                await _context.SaveChangesAsync();

                return RedirectToAction(clase.Materia);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaseExists(clase.Id)) return NotFound();
                throw;
            }
        }

        // Delete Method
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase == null) return NotFound();

            _context.Clases.Remove(clase);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Helper Methods
        private bool ClaseExists(int id)
        {
            return _context.Clases.Any(e => e.Id == id);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder, string[] allowedExtensions = null)
        {
            if (file == null || file.Length == 0) return null;

            if (allowedExtensions != null)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Solo se permiten archivos PDF o Word.");
                }
            }

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, folder);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/{folder}/{uniqueFileName}";
        }
    }
}
