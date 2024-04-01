using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;

namespace EcocargaWeb.Controllers
{
    public class ObservacionesController : Controller
    {
        private readonly EcocargaContext _context;

        public ObservacionesController(EcocargaContext context)
        {
            _context = context;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec, UserRole.Electrolinera })]
        // GET: Observaciones/Index/5
        public async Task<IActionResult> Index(int electrolineraId)
        {
            ViewData["Electrolinera"] = electrolineraId;

            var ecocargaContext = _context.DataObservacion.Include(d => d.Electrolinera)
                .Where(a => a.ElectrolineraId == electrolineraId);
            return View(await ecocargaContext.ToListAsync());
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec, UserRole.Electrolinera })]
        // GET: Observaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataObservacion = await _context.DataObservacion
                .Include(d => d.Electrolinera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataObservacion == null)
            {
                return NotFound();
            }

            return View(dataObservacion);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: Observaciones/Create/5
        public  IActionResult Create(int electrolineraId)
        {
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", electrolineraId);
            ViewData["Electrolinera"] = electrolineraId;
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Observaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mensaje,ElectrolineraId")] DataObservacion dataObservacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataObservacion);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat("Observación", Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index), new { electrolineraId = dataObservacion.ElectrolineraId });
            }
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", dataObservacion.ElectrolineraId);
            return View(dataObservacion);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Observaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataObservacion = await _context.DataObservacion.FindAsync(id);
            if (dataObservacion == null)
            {
                return NotFound();
            }

            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", dataObservacion.ElectrolineraId);
            return View(dataObservacion);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Observaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mensaje,ElectrolineraId")] DataObservacion dataObservacion)
        {
            if (id != dataObservacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataObservacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataObservacionExists(dataObservacion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat("Observación", Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index), new { electrolineraId = dataObservacion.ElectrolineraId });
            }
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", dataObservacion.ElectrolineraId);
            return View(dataObservacion);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Observaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataObservacion = await _context.DataObservacion
                .Include(d => d.Electrolinera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataObservacion == null)
            {
                return NotFound();
            }

            return View(dataObservacion);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Observaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataObservacion = await _context.DataObservacion.FindAsync(id);
            _context.DataObservacion.Remove(dataObservacion);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat("Observación", Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index), new { electrolineraId = dataObservacion.ElectrolineraId });
        }

        private bool DataObservacionExists(int id)
        {
            return _context.DataObservacion.Any(e => e.Id == id);
        }

        // GET: Observaciones/Electrolinera/5
        public IActionResult Electrolinera()
        {
            return RedirectToAction("Index", "Electrolineras");
        }
    }
}
