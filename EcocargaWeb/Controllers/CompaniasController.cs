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
using Cl.Gob.Energia.Ecocarga.Web;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using X.PagedList;
using System.Text.RegularExpressions;

namespace EcocargaWeb.Controllers
{
    public class CompaniasController : Controller
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;

        public CompaniasController(EcocargaContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _settings = settings;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
        // GET: Companias
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

            var dataCompania = from d in _context.DataCompania
                               select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                dataCompania = dataCompania.Where(d => d.Nombre.Contains(searchString));
            }

            bool sucess = Enum.TryParse(sortOrder, out TiposDeOrdenamiento sort);
            switch (sort)
            {
                case TiposDeOrdenamiento.NombreDesc:
                    dataCompania = dataCompania.OrderByDescending(d => d.Nombre);
                    break;
                default:
                    dataCompania = dataCompania.OrderBy(d => d.Nombre);
                    break;
            }

            int pageSize = Constantes.REGISTROS_PAGINADO;
            IPagedList<DataCompania> tblDataCompania = null;
            tblDataCompania = dataCompania.AsNoTracking().ToPagedList(pageNumber ?? 1, pageSize);
            return View(tblDataCompania);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
        // GET: Companias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataCompania = await _context.DataCompania
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataCompania == null)
            {
                return NotFound();
            }

            return View(dataCompania);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: Companias/Create
        public IActionResult Create()
        {
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Companias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,UrlInImage,UrlImage,Archivo,RutPropietario")] DataCompania dataCompania)
        {
            if (dataCompania.Archivo == null)
            {
                ModelState.Remove("Archivo");
            }

            if (ModelState.IsValid)
            {
                if (dataCompania.Archivo != null)
                {
                    dataCompania.UrlImage = await GuardarArchivoAsync(dataCompania.Archivo, _settings.Value.PathAntecedentes, _settings.Value.PathCompanias);
                }
                _context.Add(dataCompania);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat(dataCompania.Nombre, Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index));
            }
            return View(dataCompania);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: Companias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataCompania = await _context.DataCompania.FindAsync(id);
            if (dataCompania == null)
            {
                return NotFound();
            }
            return View(dataCompania);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // POST: Companias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,UrlInImage,UrlImage,Archivo,RutPropietario")] DataCompania dataCompania)
        {
            if (dataCompania.Archivo == null)
            {
                ModelState.Remove("Archivo");
            }

            if (id != dataCompania.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (dataCompania.Archivo != null)
                    {
                        dataCompania.UrlImage = await GuardarArchivoAsync(dataCompania.Archivo, _settings.Value.PathAntecedentes, _settings.Value.PathCompanias);
                    }
                    _context.Update(dataCompania);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataCompaniaExists(dataCompania.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat(dataCompania.Nombre, Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index));
            }
            return View(dataCompania);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Companias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataCompania = await _context.DataCompania
                .Include(de => de.DataElectrolinera)
                .Include(du => du.DataUsuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataCompania == null)
            {
                return NotFound();
            }

            if (dataCompania.DataElectrolinera.Count() > 0 || dataCompania.DataUsuario.Count() > 0)
            {
                TempData["alert"] = string.Concat(dataCompania.Nombre, Mensajes.MENSAJE_ALERTA_ELECTROLINERA);
            }

            return View(dataCompania);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Companias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataCompania = await _context.DataCompania.FindAsync(id);
            _context.DataCompania.Remove(dataCompania);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat(dataCompania.Nombre, Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index));
        }

        private bool DataCompaniaExists(int id)
        {
            return _context.DataCompania.Any(e => e.Id == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerificarRutAsync(string RutPropietario, int? id)
        {
            var dataAuto = await _context.DataCompania.FirstOrDefaultAsync(m => m.RutPropietario == RutPropietario && m.Id != id);
            if (dataAuto == null)
            {
                RutPropietario = RutPropietario.Replace(".", "").ToUpper();
                Regex expresion = new Regex("^([0-9]+-[0-9K])$");
                string dv = RutPropietario.Substring(RutPropietario.Length - 1, 1);
                if (!expresion.IsMatch(RutPropietario))
                {
                    return Json(false);
                }
                char[] charCorte = { '-' };
                string[] rutTemp = RutPropietario.Split(charCorte);
                if (dv != Digito(int.Parse(rutTemp[0])))
                {
                    return Json(false);
                }
                return Json(true);
            }
            return Json(false);
        }

        public static string Digito(int rut)
        {
            int suma = 0;
            int multiplicador = 1;
            while (rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8)
                    multiplicador = 2;
                suma += (rut % 10) * multiplicador;
                rut = rut / 10;
            }
            suma = 11 - (suma % 11);
            if (suma == 11)
            {
                return "0";
            }
            else if (suma == 10)
            {
                return "K";
            }
            else
            {
                return suma.ToString();
            }
        }
    }
}
