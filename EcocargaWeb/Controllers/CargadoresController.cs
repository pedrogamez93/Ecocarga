using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Cl.Gob.Energia.Ecocarga.Data.Utils;
using static Cl.Gob.Energia.Ecocarga.Web.Utils.Estados;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;

namespace EcocargaWeb.Controllers
{

    public class CargadoresController : Controller
    {
        private readonly EcocargaContext _context;

        public CargadoresController(EcocargaContext context)
        {
            _context = context;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
        // GET: Cargadores/Index/5
        public async Task<IActionResult> Index(int electrolineraId)
        {
            ViewData["Electrolinera"] = electrolineraId;

            var dataCargador = from d in _context.DataCargador
                               .Include(e => e.Electrolinera)
                               .Include(tc => tc.TipoConector)
                               .Where(a => a.ElectrolineraId == electrolineraId)
                                select d;

            var cargadores = await dataCargador.ToListAsync();
            cargadores.ForEach(z =>
            {
                z.TipoCorriente = z.TipoCorriente.ToUpper();
            });

            return View(cargadores);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
        // GET: Cargadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataCargador = await _context.DataCargador
                .Include(d => d.Electrolinera)
                .Include(d => d.TipoConector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataCargador == null)
            {
                return NotFound();
            }
            dataCargador.TipoCorriente = dataCargador.TipoCorriente.ToUpper();
            return View(dataCargador);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: Cargadores/Create/5
        public IActionResult Create(int electrolineraId)
        {
            ViewData["Electrolinera"] = electrolineraId;
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", electrolineraId);
            ViewData["TipoConectorId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre");
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Cargadores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cable,ElectrolineraId,TipoConectorId,TipoCorriente,Potencia,EstadoCargador,IdEstadoCargador,IdCargadorCliente,IdCargadorSec")] DataCargador dataCargador)
        {
            if (ModelState.IsValid)
            {
                EstadosCargadores enumEstado = (EstadosCargadores)dataCargador.IdEstadoCargador.Value;
                dataCargador.EstadoCargador = Estados.EnumDisplayNameFor(enumEstado).ToString();
                _context.Add(dataCargador);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat("Cargador", Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index), new { electrolineraId = dataCargador.ElectrolineraId });
            }
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", dataCargador.ElectrolineraId);
            ViewData["TipoConectorId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataCargador.TipoConectorId);
            return View(dataCargador);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera})]
        // GET: Cargadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataCargador = await _context.DataCargador.FindAsync(id);
            if (dataCargador == null)
            {
                return NotFound();
            }
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", dataCargador.ElectrolineraId);
            ViewData["TipoConectorId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataCargador.TipoConectorId);
            return View(dataCargador);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // POST: Cargadores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cable,ElectrolineraId,TipoConectorId,TipoCorriente,Potencia,EstadoCargador,IdEstadoCargador,IdCargadorCliente,IdCargadorSec")] DataCargador dataCargador)
        {
            if (id != dataCargador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    EstadosCargadores enumEstado = (EstadosCargadores)dataCargador.IdEstadoCargador.Value;
                    dataCargador.EstadoCargador = Estados.EnumDisplayNameFor(enumEstado).ToString();
                    _context.Update(dataCargador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataCargadorExists(dataCargador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat("Cargador", Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index), new { electrolineraId = dataCargador.ElectrolineraId });
            }
            ViewData["ElectrolineraId"] = new SelectList(_context.DataElectrolinera, "Id", "Nombre", dataCargador.ElectrolineraId);
            ViewData["TipoConectorId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataCargador.TipoConectorId);
            return View(dataCargador);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Cargadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataCargador = await _context.DataCargador
                .Include(d => d.Electrolinera)
                .Include(d => d.TipoConector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataCargador == null)
            {
                return NotFound();
            }
            dataCargador.TipoCorriente = dataCargador.TipoCorriente.ToUpper();
            return View(dataCargador);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Cargadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataCargador = await _context.DataCargador.FindAsync(id);
            _context.DataCargador.Remove(dataCargador);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat("Cargador", Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index), new { electrolineraId = dataCargador.ElectrolineraId });
        }

        private bool DataCargadorExists(int id)
        {
            return _context.DataCargador.Any(e => e.Id == id);
        }

        // GET: Cargadores/Electrolinera/5
        public IActionResult Electrolinera()
        {
            return RedirectToAction("Index", "Electrolineras");
        }

        // GET: Cargadores/Precio/5
        public IActionResult Precio(int id)
        {
            return RedirectToAction("Index", "TipoCobros", new { cargadorId = id });
        }
    }
}
