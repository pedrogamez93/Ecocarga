using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using static Cl.Gob.Energia.Ecocarga.Web.Utils.Funciones;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Microsoft.Extensions.Options;
using Cl.Gob.Energia.Ecocarga.Web;
using Microsoft.AspNetCore.Http;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces;
using System.Data;
using System.ComponentModel;
using X.PagedList;
using Cl.Gob.Energia.Ecocarga.Web.Models;

namespace EcocargaWeb.Controllers
{
    public class AutosController : Controller
    {
        private readonly EcocargaContext _context;
        private readonly IUserService _userService;
        private readonly IOptions<AppSettings> _settings;

        public AutosController(IOptions<AppSettings> settings, EcocargaContext context, IUserService userService)
        {
            _settings = settings;
            _context = context;
            _userService = userService;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec })]
        // GET: Autos
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ModeloSortParm"] = String.IsNullOrEmpty(sortOrder) ? TiposDeOrdenamiento.ModeloDesc.ToString() : "";
            ViewData["MarcaSortParm"] = sortOrder == TiposDeOrdenamiento.MarcaAsc.ToString() ? TiposDeOrdenamiento.MarcaDesc.ToString() : TiposDeOrdenamiento.MarcaAsc.ToString();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var dataAutos = from d in _context.DataAuto
                            .Include(m => m.Marca)
                            .Include(tca=> tca.TipoConectorAc)
                            .Include(tcd => tcd.TipoConectorDc)
                            select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                dataAutos = dataAutos.Where(d => d.Modelo.Contains(searchString)
                                       || d.Marca.Nombre.Contains(searchString));
            }

            bool sucess = Enum.TryParse(sortOrder, out TiposDeOrdenamiento sort);
            switch (sort)
            {
                case TiposDeOrdenamiento.ModeloDesc:
                    dataAutos = dataAutos.OrderByDescending(d => d.Modelo);
                    break;
                case TiposDeOrdenamiento.MarcaAsc:
                    dataAutos = dataAutos.OrderBy(d => d.Marca.Nombre);
                    break;
                case TiposDeOrdenamiento.MarcaDesc:
                    dataAutos = dataAutos.OrderByDescending(d => d.Marca.Nombre);
                    break;
                default:
                    dataAutos = dataAutos.OrderBy(d => d.Modelo);
                    break;
            }

            int pageSize = Constantes.REGISTROS_PAGINADO;
            IPagedList<DataAuto> tblDataAuto = null;
            tblDataAuto = dataAutos.AsNoTracking().ToPagedList(pageNumber ?? 1, pageSize);
            return View(tblDataAuto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Sec })]
        // GET: Autos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataAuto = await _context.DataAuto
                .Include(d => d.Marca)
                .Include(d => d.TipoConectorAc)
                .Include(d => d.TipoConectorDc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataAuto == null)
            {
                return NotFound();
            }

            return View(dataAuto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: Autos/Create
        public IActionResult Create()
        {
            ViewData["MarcaId"] = new SelectList(_context.DataMarcaauto, "Id", "Nombre");
            ViewData["TipoConectorAcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre");
            ViewData["TipoConectorDcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre");
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Autos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Modelo,Traccion,MarcaId,TipoConectorAcId,TipoConectorDcId,CapacidadBateria,CapacidadInversorInternoAc,RendimientoElectrico,Imagen,Archivo,CodigoInformeTecnico")] DataAuto dataAuto)
        {
            if (ModelState.IsValid)
            {
                if(dataAuto.TipoConectorAcId == null && dataAuto.TipoConectorDcId == null)
                {
                    ModelState.AddModelError("TipoConectorAcId", Mensajes.MENSAJE_ALERTA_TIPO_CONECTOR);
                    ModelState.AddModelError("TipoConectorDcId", Mensajes.MENSAJE_ALERTA_TIPO_CONECTOR);
                }
                else
                {
                    dataAuto.Imagen = await GuardarArchivoAsync(dataAuto.Archivo, _settings.Value.PathAntecedentes, _settings.Value.PathModelos);
                    dataAuto.IdPublico = Guid.NewGuid();
                    _context.Add(dataAuto);
                    await _context.SaveChangesAsync();
                    TempData["notice"] = string.Concat(dataAuto.Modelo, Mensajes.MENSAJE_INSERCION);
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["MarcaId"] = new SelectList(_context.DataMarcaauto, "Id", "Nombre", dataAuto.MarcaId);
            ViewData["TipoConectorAcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorAcId);
            ViewData["TipoConectorDcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorDcId);
            return View(dataAuto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Autos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataAuto = await _context.DataAuto.FindAsync(id);
            if (dataAuto == null)
            {
                return NotFound();
            }
            ViewData["MarcaId"] = new SelectList(_context.DataMarcaauto, "Id", "Nombre", dataAuto.MarcaId);
            ViewData["TipoConectorAcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorAcId);
            ViewData["TipoConectorDcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorDcId);
            return View(dataAuto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Autos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Modelo,Traccion,IdPublico,MarcaId,TipoConectorAcId,TipoConectorDcId,CapacidadBateria,CapacidadInversorInternoAc,RendimientoElectrico,Imagen,CodigoInformeTecnico,Archivo")] DataAuto dataAuto)
        {
            if (dataAuto.Archivo == null)
            {
                ModelState.Remove("Archivo");
            }

            if (id != dataAuto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (dataAuto.TipoConectorAcId == null && dataAuto.TipoConectorDcId == null)
                    {
                        ModelState.AddModelError("TipoConectorAcId", Mensajes.MENSAJE_ALERTA_TIPO_CONECTOR);
                        ModelState.AddModelError("TipoConectorDcId", Mensajes.MENSAJE_ALERTA_TIPO_CONECTOR);
                        ViewData["MarcaId"] = new SelectList(_context.DataMarcaauto, "Id", "Nombre", dataAuto.MarcaId);
                        ViewData["TipoConectorAcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorAcId);
                        ViewData["TipoConectorDcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorDcId);
                        return View(dataAuto);
                    }

                    if (dataAuto.Archivo != null)
                    {
                        dataAuto.Imagen = await GuardarArchivoAsync(dataAuto.Archivo, _settings.Value.PathAntecedentes, _settings.Value.PathModelos);
                    }
                    _context.Update(dataAuto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataAutoExists(dataAuto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat(dataAuto.Modelo, Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MarcaId"] = new SelectList(_context.DataMarcaauto, "Id", "Nombre", dataAuto.MarcaId);
            ViewData["TipoConectorAcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorAcId);
            ViewData["TipoConectorDcId"] = new SelectList(_context.DataTipoconector, "Id", "Nombre", dataAuto.TipoConectorDcId);
            return View(dataAuto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Autos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataAuto = await _context.DataAuto
                .Include(d => d.Marca)
                .Include(d => d.TipoConectorAc)
                .Include(d => d.TipoConectorDc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataAuto == null)
            {
                return NotFound();
            }

            return View(dataAuto);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Autos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataAuto = await _context.DataAuto.FindAsync(id);
            _context.DataAuto.Remove(dataAuto);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat(dataAuto.Modelo, Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index));
        }

        private bool DataAutoExists(int id)
        {
            return _context.DataAuto.Any(e => e.Id == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerificarNombreAsync(string modelo, int? id)
        {
            var dataAuto = await _context.DataAuto.FirstOrDefaultAsync(m => m.Modelo == modelo && m.Id != id);
            if (dataAuto == null)
            {
                return Json(true);
            }
            return Json(false);
        }

        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Autos/Create
        public async Task<FileResult> DownloadXLSAsync(string currentFilter)
        {
            var dataAutos = _context.DataAuto
            .Include(d => d.Marca)
            .Include(d => d.TipoConectorAc)
            .Include(d => d.TipoConectorDc)
            .Select(c => new ExportarAuto()
            {
                Id = c.Id,
                Marca = c.Marca.Nombre,
                Modelo = c.Modelo,
                Traccion = c.Traccion,
                CapacidadBateria = c.CapacidadBateria,
                CapacidadInversorInternoAC = c.CapacidadInversorInternoAc,
                RendimientoElectrico = c.RendimientoElectrico,
                ConectorAC = c.TipoConectorAc.Nombre,
                ConectorDC = c.TipoConectorDc.Nombre,
                CodigoInformeTecnico = c.CodigoInformeTecnico,
            }).OrderBy(m => m.Marca).ThenBy(m => m.Modelo);

            if (!String.IsNullOrEmpty(currentFilter))
            {
                dataAutos = dataAutos.Where(d => d.Modelo.Contains(currentFilter)
                                       || d.Marca.Contains(currentFilter)).OrderBy(m => m.Marca).ThenBy(m => m.Modelo);
            }

            var result = await dataAutos.ToListAsync();

            //string[] ignoredColumns = { "Archivo" };

            byte[] filecontent = ExcelExportHelper.ExportExcel(result, "EcocargaWeb - Autos"/*, ignoredColumns*/);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Autos.xlsx");
        }
    }
}
