using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using static Cl.Gob.Energia.Ecocarga.Web.Utils.Funciones;
using Microsoft.Extensions.Options;
using Cl.Gob.Energia.Ecocarga.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using X.PagedList;

namespace EcocargaWeb.Controllers
{
    public class MarcasAutosController : Controller
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;

        public MarcasAutosController(EcocargaContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _settings = settings;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec })]
        // GET: MarcasAutos
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

            var dataMarcAuto = from d in _context.DataMarcaauto
                                select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                dataMarcAuto = dataMarcAuto.Where(d => d.Nombre.Contains(searchString));
            }

            bool sucess = Enum.TryParse(sortOrder, out TiposDeOrdenamiento sort);
            switch (sort)
            {
                case TiposDeOrdenamiento.NombreDesc:
                    dataMarcAuto = dataMarcAuto.OrderByDescending(d => d.Nombre);
                    break;
                default:
                    dataMarcAuto = dataMarcAuto.OrderBy(d => d.Nombre);
                    break;
            }

            int pageSize = Constantes.REGISTROS_PAGINADO;
            IPagedList<DataMarcaauto> tblDataMarcaauto = null;
            tblDataMarcaauto = dataMarcAuto.AsNoTracking().ToPagedList(pageNumber ?? 1, pageSize);
            return View(tblDataMarcaauto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec })]
        // GET: MarcasAutos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataMarcaauto = await _context.DataMarcaauto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataMarcaauto == null)
            {
                return NotFound();
            }

            return View(dataMarcaauto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: MarcasAutos/Create
        public IActionResult Create()
        {
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: MarcasAutos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Imagen,Archivo")] DataMarcaauto dataMarcaauto)
        {
            if (ModelState.IsValid)
            {
                dataMarcaauto.Imagen = await GuardarArchivoAsync(dataMarcaauto.Archivo, _settings.Value.PathAntecedentes, _settings.Value.PathMarcas);
                _context.Add(dataMarcaauto);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat(dataMarcaauto.Nombre, Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index));
            }
            return View(dataMarcaauto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: MarcasAutos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataMarcaauto = await _context.DataMarcaauto.FindAsync(id);
            if (dataMarcaauto == null)
            {
                return NotFound();
            }
            return View(dataMarcaauto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: MarcasAutos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Imagen,Archivo")] DataMarcaauto dataMarcaauto/*, IFormFile fileUpdate*/)
        {
            if (dataMarcaauto.Archivo == null)
            {
                ModelState.Remove("Archivo");
            }

            if (id != dataMarcaauto.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (dataMarcaauto.Archivo != null)
                    {
                        dataMarcaauto.Imagen = await GuardarArchivoAsync(dataMarcaauto.Archivo, _settings.Value.PathAntecedentes, _settings.Value.PathMarcas);
                    }
                    _context.Update(dataMarcaauto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataMarcaautoExists(dataMarcaauto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat(dataMarcaauto.Nombre, Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index));
            }
            return View(dataMarcaauto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: MarcasAutos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataMarcaauto = await _context.DataMarcaauto
                .Include(da => da.DataAuto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataMarcaauto == null)
            {
                return NotFound();
            }

            if (dataMarcaauto.DataAuto.Count() > 0)
            {
                TempData["alert"] = string.Concat(dataMarcaauto.Nombre, Mensajes.MENSAJE_ALERTA_AUTO);
            }

            return View(dataMarcaauto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: MarcasAutos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataMarcaauto = await _context.DataMarcaauto.FindAsync(id);
            _context.DataMarcaauto.Remove(dataMarcaauto);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat(dataMarcaauto.Nombre, Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index));
        }

        private bool DataMarcaautoExists(int id)
        {
            return _context.DataMarcaauto.Any(e => e.Id == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerificarNombreAsync(string nombre, int? id)
        {
            var dataMarcaauto = await _context.DataMarcaauto.FirstOrDefaultAsync(m => m.Nombre == nombre && m.Id != id);
            if (dataMarcaauto == null)
            {
                return Json(true);
            }
            return Json(false);
        }
    }
}
