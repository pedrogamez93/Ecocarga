using Ecocarga.Data;
using Ecocarga.Models;
using Ecocarga.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class TermsAndConditionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserActionService _userActionService;
    public TermsAndConditionsController(ApplicationDbContext context, UserActionService userActionService)
    {
        _context = context;
        _userActionService = userActionService; // Asignación del servicio de registro de acciones
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var terms = _context.TermsAndConditions;

        // Registrar la acción de visualización de términos y condiciones
        await _userActionService.LogActionAsync("Visualización de Términos y Condiciones", "El usuario ha visualizado la lista de términos y condiciones.");

        return View(terms);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TermsAndConditions model)
    {
        if (ModelState.IsValid)
        {
            model.CreatedAt = DateTime.Now;
            _context.TermsAndConditions.Add(model);
            await _context.SaveChangesAsync();

            // Registrar la acción de creación de términos y condiciones
            await _userActionService.LogActionAsync("Creación de Términos y Condiciones", $"El usuario ha creado los términos y condiciones: {model.Id}");

            return RedirectToAction("Index");
        }
        return View(model);
    }
    // Métodos adicionales para editar y eliminar términos y condiciones

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var term = await _context.TermsAndConditions.FindAsync(id);
        if (term == null)
        {
            return NotFound();
        }

        // Registrar la acción de edición de términos y condiciones (al visualizar la vista de edición)
        await _userActionService.LogActionAsync("Edición de Términos y Condiciones", $"El usuario está editando los términos y condiciones: {term.Id}");

        return View(term);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TermsAndConditions model)
    {
        if (ModelState.IsValid)
        {
            var term = await _context.TermsAndConditions.FindAsync(model.Id);
            if (term == null)
            {
                return NotFound();
            }

            term.Content = model.Content;
            term.UpdatedAt = DateTime.Now;

            _context.TermsAndConditions.Update(term);
            await _context.SaveChangesAsync();

            // Registrar la acción de actualización de términos y condiciones
            await _userActionService.LogActionAsync("Actualización de Términos y Condiciones", $"El usuario ha actualizado los términos y condiciones: {term.Id}");

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var term = await _context.TermsAndConditions.FindAsync(id);
        if (term == null)
        {
            return NotFound();
        }

        // Registrar la acción de visualización de la vista de eliminación de términos y condiciones
        await _userActionService.LogActionAsync("Eliminación de Términos y Condiciones", $"El usuario ha accedido a la eliminación de los términos y condiciones: {term.Id}");

        return View(term);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var term = await _context.TermsAndConditions.FindAsync(id);
        if (term == null)
        {
            return NotFound();
        }

        _context.TermsAndConditions.Remove(term);
        await _context.SaveChangesAsync();

        // Registrar la acción de eliminación de términos y condiciones
        await _userActionService.LogActionAsync("Eliminación de Términos y Condiciones", $"El usuario ha eliminado los términos y condiciones: {term.Id}");

        return RedirectToAction("Index");
    }
}
