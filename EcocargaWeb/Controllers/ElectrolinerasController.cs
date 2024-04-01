using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using static Cl.Gob.Energia.Ecocarga.Web.Utils.Estados;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces;
using X.PagedList;
using Cl.Gob.Energia.Ecocarga.Web.Models;

namespace EcocargaWeb.Controllers
{
    public class ElectrolinerasController : Controller
    {
        private readonly EcocargaContext _context;
        private readonly IUserService _userService;

        public ElectrolinerasController(EcocargaContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
        // GET: Electrolineras
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? TiposDeOrdenamiento.NombreDesc.ToString() : "";
            ViewData["CompaniaSortParm"] = sortOrder == TiposDeOrdenamiento.CompaniaAsc.ToString() ? TiposDeOrdenamiento.CompaniaDesc.ToString() : TiposDeOrdenamiento.CompaniaAsc.ToString();
            ViewData["RegionSortParm"] = sortOrder == TiposDeOrdenamiento.RegionAsc.ToString() ? TiposDeOrdenamiento.RegionDesc.ToString() : TiposDeOrdenamiento.RegionAsc.ToString();
            ViewData["ComunaSortParm"] = sortOrder == TiposDeOrdenamiento.ComunaAsc.ToString() ? TiposDeOrdenamiento.ComunaDesc.ToString() : TiposDeOrdenamiento.ComunaAsc.ToString();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var dataElectrolineras = from d in _context.DataElectrolinera
                                    .Include(c => c.Compania)
                                     select d;

            if (_userService.IsElectrolinera())
            {
                User user = _userService.GetUser();
                dataElectrolineras = dataElectrolineras.Where(d => d.Compania.Nombre.Equals(user.Empresa));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                dataElectrolineras = dataElectrolineras.Where(d => d.Nombre.Contains(searchString)
                                     || d.Compania.Nombre.Contains(searchString)
                                     || d.Region.Contains(searchString)
                                     || d.Comuna.Contains(searchString));
            }

            bool sucess = Enum.TryParse(sortOrder, out TiposDeOrdenamiento sort);
            switch (sort)
            {
                case TiposDeOrdenamiento.NombreDesc:
                    dataElectrolineras = dataElectrolineras.OrderByDescending(d => d.Nombre);
                    break;
                case TiposDeOrdenamiento.CompaniaAsc:
                    dataElectrolineras = dataElectrolineras.OrderBy(d => d.Compania.Nombre);
                    break;
                case TiposDeOrdenamiento.CompaniaDesc:
                    dataElectrolineras = dataElectrolineras.OrderByDescending(d => d.Compania.Nombre);
                    break;
                case TiposDeOrdenamiento.RegionAsc:
                    dataElectrolineras = dataElectrolineras.OrderBy(d => d.Region);
                    break;
                case TiposDeOrdenamiento.RegionDesc:
                    dataElectrolineras = dataElectrolineras.OrderByDescending(d => d.Region);
                    break;
                case TiposDeOrdenamiento.ComunaAsc:
                    dataElectrolineras = dataElectrolineras.OrderBy(d => d.Comuna);
                    break;
                case TiposDeOrdenamiento.ComunaDesc:
                    dataElectrolineras = dataElectrolineras.OrderByDescending(d => d.Comuna);
                    break;
                default:
                    dataElectrolineras = dataElectrolineras.OrderBy(d => d.Nombre);
                    break;
            }
            int pageSize = Constantes.REGISTROS_PAGINADO;

            IPagedList<DataElectrolinera> tblDataElectrolinera = null;
            tblDataElectrolinera = dataElectrolineras.AsNoTracking().ToPagedList(pageNumber ?? 1, pageSize);
            return View(tblDataElectrolinera);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
        // GET: Electrolineras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataElectrolinera = await _context.DataElectrolinera
                .Include(d => d.Compania)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataElectrolinera == null)
            {
                return NotFound();
            }

            return View(dataElectrolinera);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador})]
        // GET: Electrolineras/Create
        public IActionResult Create()
        {
            ViewData["CompaniaId"] = new SelectList(_context.DataCompania, "Id", "Nombre");
            return View();
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Electrolineras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Latitud,Longitud,Direccion,CantidadPuntosCarga,Marca,Potencia,Precio,Horario,Comuna,Region,CompaniaId,AceptaConexionAc,AceptaConexionDc,EstadoElectrolinera,Modelo,IdElectrolineraCliente,IdEstadoElectrolinera,IdElectrolineraSec,CoordenadasActualizar")] DataElectrolinera dataElectrolinera)
        {
            if (ModelState.IsValid)
            {
                dataElectrolinera.IdPublico = Guid.NewGuid();
                EstadosCargadores enumEstado = (EstadosCargadores)dataElectrolinera.IdEstadoElectrolinera.Value;
                dataElectrolinera.EstadoElectrolinera = Estados.EnumDisplayNameFor(enumEstado).ToString();
                _context.Add(dataElectrolinera);
                await _context.SaveChangesAsync();
                TempData["notice"] = string.Concat(dataElectrolinera.Nombre, Mensajes.MENSAJE_INSERCION);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompaniaId"] = new SelectList(_context.DataCompania, "Id", "Nombre", dataElectrolinera.CompaniaId);
            return View(dataElectrolinera);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera})]
        // GET: Electrolineras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataElectrolinera = await _context.DataElectrolinera.FindAsync(id);
            if (dataElectrolinera == null)
            {
                return NotFound();
            }
            ViewData["CompaniaId"] = new SelectList(_context.DataCompania, "Id", "Nombre", dataElectrolinera.CompaniaId);
            return View(dataElectrolinera);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera })]
        // POST: Electrolineras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Latitud,Longitud,Direccion,CantidadPuntosCarga,Marca,Potencia,Precio,Horario,Comuna,Region,IdPublico,CompaniaId,AceptaConexionAc,AceptaConexionDc,EstadoElectrolinera,Modelo,IdElectrolineraCliente,IdEstadoElectrolinera,IdElectrolineraSec,CoordenadasActualizar")] DataElectrolinera dataElectrolinera)
        {
            if (id != dataElectrolinera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    EstadosCargadores enumEstado = (EstadosCargadores)dataElectrolinera.IdEstadoElectrolinera.Value;
                    dataElectrolinera.EstadoElectrolinera = Estados.EnumDisplayNameFor(enumEstado).ToString();
                    _context.Update(dataElectrolinera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataElectrolineraExists(dataElectrolinera.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["notice"] = string.Concat(dataElectrolinera.Nombre, Mensajes.MENSAJE_ACTUALIZACION);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompaniaId"] = new SelectList(_context.DataCompania, "Id", "Nombre", dataElectrolinera.CompaniaId);
            return View(dataElectrolinera);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Electrolineras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataElectrolinera = await _context.DataElectrolinera
                .Include(d => d.Compania)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataElectrolinera == null)
            {
                return NotFound();
            }

            return View(dataElectrolinera);
        }
        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // POST: Electrolineras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataElectrolinera = await _context.DataElectrolinera.FindAsync(id);
            _context.DataElectrolinera.Remove(dataElectrolinera);
            await _context.SaveChangesAsync();
            TempData["notice"] = string.Concat(dataElectrolinera.Nombre, Mensajes.MENSAJE_BORRADO);
            return RedirectToAction(nameof(Index));
        }

        private bool DataElectrolineraExists(int id)
        {
            return _context.DataElectrolinera.Any(e => e.Id == id);
        }

        // GET: Electrolineras/Observacion/5
        public IActionResult Observacion(int id)
        {
            return RedirectToAction("Index", "Observaciones", new { electrolineraId = id });
        }

        // GET: Electrolineras/Cargador/5
        public IActionResult Cargador(int id)
        {
            return RedirectToAction("Index", "Cargadores", new { electrolineraId = id });
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerificarNombreAsync(string nombre, int? id)
        {
            var dataElectrolinera = await _context.DataElectrolinera.FirstOrDefaultAsync(m => m.Nombre == nombre && m.Id != id);
            if (dataElectrolinera == null)
            {
                return Json(true);
            }
            return Json(false);
        }

        [AllowedRoles(new UserRole[] { UserRole.Administrador })]
        // GET: Autos/Create
        public async Task<FileResult> DownloadXLSAsync(string currentFilter)
        {
            var dataCargador = _context.DataCargador
                               .Include(e => e.Electrolinera)
                               .Include(tc => tc.TipoConector)
                               .Select(c => new ExportarElectrolinera()
                               {
                                   Id = c.Electrolinera.Id,
                                   NombreElectrolinera = c.Electrolinera.Nombre,
                                   Compania = c.Electrolinera.Compania.Nombre,
                                   Region = c.Electrolinera.Region,
                                   Comuna = c.Electrolinera.Comuna,
                                   Direccion = c.Electrolinera.Direccion,
                                   Latitud = c.Electrolinera.Latitud,
                                   Longitud = c.Electrolinera.Longitud,
                                   PuntosCarga = c.Electrolinera.CantidadPuntosCarga,
                                   Marca = c.Electrolinera.Marca,
                                   Modelo = c.Electrolinera.Modelo,
                                   Potencia = c.Electrolinera.Potencia,
                                   AceptaConexionAC = c.Electrolinera.AceptaConexionAc,
                                   AceptaConexionDC = c.Electrolinera.AceptaConexionDc,
                                   Horario = c.Electrolinera.Horario,
                                   EstadoElectrolinera = c.Electrolinera.EstadoElectrolinera,
                                   IdElectrolineraCliente = c.Electrolinera.IdElectrolineraCliente,
                                   IdElectrolineraSEC = c.Electrolinera.IdElectrolineraSec,
                                   IdCargador = c.Id,
                                   IdCargadorCliente = c.IdCargadorCliente,
                                   IdCargadorSEC = c.IdCargadorSec,
                                   TipoConector = c.TipoConector.Nombre,
                                   TipoCorriente = c.TipoCorriente.ToUpper(),
                                   Cable = c.Cable,
                                   PotenciaCargador = c.Potencia,
                                   EstadoCargador = c.EstadoCargador,
                               }).OrderBy(m => m.Compania).ThenBy(m => m.NombreElectrolinera);

            if (!String.IsNullOrEmpty(currentFilter))
            {
                dataCargador = dataCargador.Where(d => d.NombreElectrolinera.Contains(currentFilter)
                                     || d.Compania.Contains(currentFilter)
                                     || d.Region.Contains(currentFilter)
                                     || d.Comuna.Contains(currentFilter)).OrderBy(m => m.Compania).ThenBy(m => m.NombreElectrolinera);
            }

            var result = await dataCargador.ToListAsync();

            //string[] ignoredColumns = { "Archivo" };

            byte[] filecontent = ExcelExportHelper.ExportExcel(result, "EcocargaWeb - Electrolineras"/*, ignoredColumns*/);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Electrolineras.xlsx");
        }
    }
}
