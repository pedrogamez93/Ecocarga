using Ecocarga.Data;
using Ecocarga.Models;
using Microsoft.AspNetCore.Mvc;


using CsvHelper;
using Ecocarga.Data.Dtos;
using System.Globalization;
using Ecocarga.Services;

namespace Ecocarga.Controllers
{
    public class BateriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserActionService _userActionService;

        public BateriasController(ApplicationDbContext context, UserActionService userActionService)
        {
            _context = context;
            _userActionService = userActionService;
        }

        [HttpGet]
        public async Task<IActionResult> Cargar()
        {
            await _userActionService.LogActionAsync("Acceso a módulo de baterías", "El usuario ha accedido a la página para cargar un CSV de baterías.");

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Cargar(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ",", // O usa ";" si ese es tu delimitador.
                        HasHeaderRecord = true, // Si tu CSV tiene encabezados, mantener esto como 'true'
                    }))
                    {
                        // Lee los registros como decimales
                        var bateriaDtos = csv.GetRecords<BateriaDto>().ToList();

                        // Convierte los DTOs a entidades y guarda en la base de datos
                        var baterias = bateriaDtos.Select(dto => new Bateria
                        {
                            Capacidad = dto.Capacidad // Decimal
                        }).ToList();

                        // Guardar en la base de datos
                        _context.Baterias.AddRange(baterias);
                        await _context.SaveChangesAsync();

                        // Registrar la acción del usuario
                        await _userActionService.LogActionAsync("Carga de baterías", $"El usuario ha cargado {baterias.Count} registros de baterías desde un archivo CSV.");
                    }

                    return RedirectToAction("Lista"); // Redirige a la lista de baterías después de la carga exitosa
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error procesando el archivo CSV: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError("", "No se seleccionó ningún archivo o el archivo está vacío.");
            }

            return View(); // En caso de error, vuelve a la vista Cargar
        }

        public async Task<IActionResult> Lista()
        {
            // Registrar la acción del usuario
            await _userActionService.LogActionAsync("Acceso a lista de baterías", "El usuario ha accedido a la lista de baterías.");

            var baterias = _context.Baterias.ToList();
            return View(baterias);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarTodo()
        {
            try
            {
                // Registrar la acción del usuario
                await _userActionService.LogActionAsync("Eliminación de baterías", "El usuario ha eliminado todos los registros de baterías.");

                // Eliminar todos los registros de la tabla Baterias
                _context.Baterias.RemoveRange(_context.Baterias);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Redirigir a la lista después de eliminar
                return RedirectToAction("Lista");
            }
            catch (Exception ex)
            {
                // Manejar errores y mostrar un mensaje en la vista
                ModelState.AddModelError("", $"Error eliminando los registros: {ex.Message}");
                return View("Lista");
            }
        }

    }
}
