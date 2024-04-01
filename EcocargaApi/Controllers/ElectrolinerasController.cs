using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Microsoft.Extensions.Options;
using Cl.Gob.Energia.Ecocarga.Api;
using Microsoft.Extensions.Logging;

namespace EcocargaApi.Controllers
{
    [Route("api/electrolineras")]
    [Produces("application/json")]
    [ApiController]
    public class ElectrolinerasController : ControllerBase
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger<ElectrolinerasController> _logger;

        public ElectrolinerasController(EcocargaContext context, IOptions<AppSettings> settings, ILogger<ElectrolinerasController> logger)
        {
            _context = context;
            _settings = settings;
            _logger = logger;
        }
        /// <summary>
        /// Listado de todas las electrolineras
        /// </summary>
        /// <returns></returns>
        // GET: api/Electrolineras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataElectrolinera>>> GetDataElectrolinera()
        {
            var path = _settings.Value.PathAntecedentes.ToString();

            var dataElectrolinera = await _context.DataElectrolinera
                                    .Include(c => c.Compania)
                                    .Include(dc => dc.DataCargador)
                                        .ThenInclude(dtc => dtc.DataTipocobroCargador)
                                    .Include(dca => dca.DataCargador)
                                        .ThenInclude(dtc => dtc.TipoConector)
                                    .Include(dob => dob.DataObservacion)
                                    .ToListAsync();


            dataElectrolinera.ForEach(e=>
            {
                if (!String.IsNullOrEmpty(e.Compania.UrlImage) && !e.Compania.UrlImage.Contains(path))
                {
                    e.Compania.UrlImage = string.Concat(path, e.Compania.UrlImage);
                }
            });

            return Ok(dataElectrolinera);
        }
        /// <summary>
        /// Obtiene Electrolinera por Id
        /// </summary>
        /// <param name="id">Id Electrolinera</param>
        /// <returns></returns>
        // GET: api/Electrolineras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DataElectrolinera>> GetDataElectrolinera(int id)
        {
            var path = _settings.Value.PathAntecedentes.ToString();

            var dataElectrolinera = await _context.DataElectrolinera
                .Where(e => e.Id == id)
                .Include(c => c.Compania)
                .Include(dc => dc.DataCargador)
                    .ThenInclude(tcc => tcc.DataTipocobroCargador)
                .Include(dca => dca.DataCargador)
                    .ThenInclude(tc => tc.TipoConector)
                .Include(o => o.DataObservacion)
                .FirstAsync();

            if (dataElectrolinera == null)
            {
                return NotFound();
            }
            else
            {
                if (!String.IsNullOrEmpty(dataElectrolinera.Compania.UrlImage) && !dataElectrolinera.Compania.UrlImage.Contains(path))
                {
                    dataElectrolinera.Compania.UrlImage = string.Concat(path, dataElectrolinera.Compania.UrlImage);
                }
            }

            return Ok(dataElectrolinera);
        }

        /// <summary>
        /// Obtener electrolineras por rango
        /// </summary>
        /// <param name="latitud">Latitud de ubicación</param>
        /// <param name="longitud">Longitud de ubicación</param>
        /// <param name="deltaLatitud">Rango de desplazamiento latitud</param>
        /// <param name="deltaLongitud">Rango de desplazamiento longitud</param>
        /// <returns></returns>
        // GET: api/Electrolineras/rango-electrolineras/5
        [HttpGet("rango-electrolineras")]
        public async Task<ActionResult<DataElectrolinera>> GetDataElectrolinera([FromQuery] Double latitud, [FromQuery] Double longitud , [FromQuery] Double deltaLatitud, [FromQuery] Double deltaLongitud)
        {
            var path = _settings.Value.PathAntecedentes.ToString();
            double latitudInferior = latitud - deltaLatitud;
            double latitudSuperior = latitud + deltaLatitud;
            double longitudInferior = longitud - deltaLongitud;
            double longitudSuperior = longitud + deltaLongitud;

            var dataElectrolinera = await _context.DataElectrolinera
                .Where(e => e.Latitud > latitudInferior 
                    && e.Latitud < latitudSuperior 
                    && e.Longitud > longitudInferior 
                    && e.Longitud < longitudSuperior)
                .Include(c => c.Compania)
                .Include(dc => dc.DataCargador)
                    .ThenInclude(tcc => tcc.DataTipocobroCargador)
                .Include(dca => dca.DataCargador)
                    .ThenInclude(tc => tc.TipoConector)
                .Include(o => o.DataObservacion)
                .ToListAsync();

            if (dataElectrolinera == null)
            {
                return NotFound();
            }
            else
            {
                dataElectrolinera.ToList().ForEach(e =>
                {
                    if (!String.IsNullOrEmpty(e.Compania.UrlImage) && !e.Compania.UrlImage.Contains(path))
                    {
                        e.Compania.UrlImage = string.Concat(path, e.Compania.UrlImage);
                    }
                });
            }

            return Ok(dataElectrolinera);
        }
    }
}
