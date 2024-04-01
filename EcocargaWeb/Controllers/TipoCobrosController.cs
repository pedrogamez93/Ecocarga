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
    public class TipoCobrosController : Controller
    {
        private readonly EcocargaContext _context;

        public TipoCobrosController(EcocargaContext context)
        {
            _context = context;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec, UserRole.Electrolinera })]
        // GET: TipoCobros
        public async Task<IActionResult> Index(int cargadorId)
        {
            var dataTipocobro = _context.DataTipocobro.Include(d => d.Cargador)
                                  .Where(a => a.CargadorId == cargadorId)
                                  .ToListAsync();

            var dataCargador = _context.DataCargador.FirstOrDefaultAsync(m => m.Id == cargadorId);

            await Task.WhenAll(dataTipocobro, dataCargador);

            ViewBag.Flag = dataTipocobro.Result.Count() == 0 ? true : false;
            ViewData["Cargador"] = cargadorId;
            ViewData["Electrolinera"] = dataCargador.Result.ElectrolineraId;
            return View(dataTipocobro.Result);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec, UserRole.Electrolinera })]
        // GET: TipoCobros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTipocobro = await _context.DataTipocobro
                .Include(d => d.Cargador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataTipocobro == null)
            {
                return NotFound();
            }

            return View(dataTipocobro);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // GET: TipoCobros/Create
        public IActionResult Create(int cargadorId)
        {
            ViewData["Cargador"] = cargadorId;
            ViewData["CargadorId"] = new SelectList(_context.DataCargador, "Id", "TipoCorriente", cargadorId);
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // POST: TipoCobros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,UnidadCobro,CargadorId,Precio")] DataTipocobro dataTipocobro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataTipocobro);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat(dataTipocobro.Nombre, Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index), new { cargadorId = dataTipocobro.CargadorId });
            }

            return View(dataTipocobro);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // GET: TipoCobros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTipocobro = await _context.DataTipocobro.FindAsync(id);
            if (dataTipocobro == null)
            {
                return NotFound();
            }

            return View(dataTipocobro);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // POST: TipoCobros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,UnidadCobro,CargadorId,Precio")] DataTipocobro dataTipocobro)
        {
            if (id != dataTipocobro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataTipocobro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataTipocobroExists(dataTipocobro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat(dataTipocobro.Nombre, Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index), new { cargadorId = dataTipocobro.CargadorId });
            }

            return View(dataTipocobro);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // GET: TipoCobros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTipocobro = await _context.DataTipocobro
                .Include(d => d.Cargador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataTipocobro == null)
            {
                return NotFound();
            }

            return View(dataTipocobro);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // POST: TipoCobros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataTipocobro = await _context.DataTipocobro.FindAsync(id);
            _context.DataTipocobro.Remove(dataTipocobro);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat(dataTipocobro.Nombre, Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index), new { cargadorId = dataTipocobro.CargadorId });
        }

        private bool DataTipocobroExists(int id)
        {
            return _context.DataTipocobro.Any(e => e.Id == id);
        }

        // GET: TipoCobros/Cargador/5
        public IActionResult Cargador(int electrolineraId)
        {
            return RedirectToAction("Index", "Cargadores", new { electrolineraId });
        }
    }
}
