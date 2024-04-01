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
using X.PagedList;

namespace EcocargaWeb.Controllers
{
    public class TiposConectoresController : Controller
    {
        private readonly EcocargaContext _context;

        public TiposConectoresController(EcocargaContext context)
        {
            _context = context;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec })]
        // GET: TiposConectores
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? TiposDeOrdenamiento.NombreDesc.ToString() : "";
           
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var dataTipoconector = from d in _context.DataTipoconector
                                      select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                dataTipoconector = dataTipoconector.Where(d => d.Nombre.Contains(searchString));
            }

            bool sucess = Enum.TryParse(sortOrder, out TiposDeOrdenamiento sort);
            switch (sort)
            {
                case TiposDeOrdenamiento.NombreDesc:
                    dataTipoconector = dataTipoconector.OrderByDescending(d => d.Nombre);
                    break;
                default:
                    dataTipoconector = dataTipoconector.OrderBy(d => d.Nombre);
                    break;
            }

            int pageSize = Constantes.REGISTROS_PAGINADO;
            IPagedList<DataTipoconector> tblDataTipoconector = null;
            tblDataTipoconector = dataTipoconector.AsNoTracking().ToPagedList(pageNumber ?? 1, pageSize);
            return View(tblDataTipoconector);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec })]
        // GET: TiposConectores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTipoconector = await _context.DataTipoconector
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataTipoconector == null)
            {
                return NotFound();
            }

            return View(dataTipoconector);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: TiposConectores/Create
        public IActionResult Create()
        {
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: TiposConectores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,IdPublico")] DataTipoconector dataTipoconector)
        {
            if (ModelState.IsValid)
            {
                dataTipoconector.IdPublico = Guid.NewGuid();
                _context.Add(dataTipoconector);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat(dataTipoconector.Nombre, Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index));
            }
            return View(dataTipoconector);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: TiposConectores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTipoconector = await _context.DataTipoconector.FindAsync(id);
            if (dataTipoconector == null)
            {
                return NotFound();
            }
            return View(dataTipoconector);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: TiposConectores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,IdPublico")] DataTipoconector dataTipoconector)
        {
            if (id != dataTipoconector.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataTipoconector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataTipoconectorExists(dataTipoconector.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat(dataTipoconector.Nombre, Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index));
            }
            return View(dataTipoconector);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: TiposConectores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataTipoconector = await _context.DataTipoconector
                .Include(dc => dc.DataCargador)
                .Include(dadc => dadc.DataAutoTipoConectorDc)
                .Include(dadc => dadc.DataAutoTipoConectorAc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataTipoconector == null)
            {
                return NotFound();
            }

            if (dataTipoconector.DataCargador.Count() > 0 || dataTipoconector.DataAutoTipoConectorAc.Count() > 0 || dataTipoconector.DataAutoTipoConectorDc.Count() > 0)
            {
                TempData["alert"] = string.Concat(dataTipoconector.Nombre, Mensajes.MENSAJE_ALERTA_AUTO_CARGADORES);
            }

            return View(dataTipoconector);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: TiposConectores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataTipoconector = await _context.DataTipoconector.FindAsync(id);
            _context.DataTipoconector.Remove(dataTipoconector);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat(dataTipoconector.Nombre, Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index));
        }

        private bool DataTipoconectorExists(int id)
        {
            return _context.DataTipoconector.Any(e => e.Id == id);
        }
    }
}
