using Ecocarga.Data;
using Ecocarga.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class TermsAndConditionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TermsAndConditionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var terms = _context.TermsAndConditions;
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
        return RedirectToAction("Index");
    }
}
