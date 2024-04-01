using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Api;
using Microsoft.Extensions.Options;

namespace EcocargaApi.Controllers
{
    [Route("api/autos")]
    [Produces("application/json")]
    [ApiController]
    public class AutosController : ControllerBase
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;

        public AutosController(EcocargaContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _settings = settings;
        }
        /// <summary>
        /// Listado de todos los autos agrupados por Marca
        /// </summary>
        /// <returns></returns>
        // GET: api/autos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataMarcaauto>>> GetDataMarcaAuto()
        {
            var path = _settings.Value.PathAntecedentes.ToString();

            var dataMarcaauto = await _context.DataMarcaauto
                                .Include(a => a.DataAuto)
                                    .ThenInclude(tpa => tpa.TipoConectorAc)
                                .Include(a => a.DataAuto)
                                    .ThenInclude(tpd => tpd.TipoConectorDc)
                                .ToListAsync();

            dataMarcaauto.ToList().ForEach(ma=>
            {
                if (!String.IsNullOrEmpty(ma.Imagen))
                {
                    ma.Imagen = string.Concat(path, ma.Imagen);
                }

                ma.DataAuto.ToList().ForEach(a =>
                {
                    if (!String.IsNullOrEmpty(a.Imagen))
                    {
                        a.Imagen = string.Concat(path, a.Imagen);
                    }
                });
            });
            return Ok(dataMarcaauto);
        }
        /// <summary>
        /// Obtiene autos por Id de Marca
        /// </summary>
        /// <param name="id">Id de Marca</param>
        /// <returns></returns>
        // GET: api/autos/5
        [HttpGet("{id}", Name = "idMarca")]
        public async Task<ActionResult<DataMarcaauto>> GetDataMarcaAuto(int id)
        {
            var path = _settings.Value.PathAntecedentes.ToString();

            var dataMarcaauto = await _context.DataMarcaauto
                                .Where(ma => ma.Id == id)
                                .Include(a => a.DataAuto)
                                    .ThenInclude(tca => tca.TipoConectorAc)
                                .Include(a => a.DataAuto)
                                    .ThenInclude(tcd => tcd.TipoConectorDc)
                                .FirstAsync();

            if (dataMarcaauto == null)
            {
                return NotFound();
            }
            else
            { 
                dataMarcaauto.Imagen = string.Concat(path, dataMarcaauto.Imagen);

                dataMarcaauto.DataAuto.ToList().ForEach(ma =>
                {
                    if (!String.IsNullOrEmpty(ma.Imagen))
                    {
                        ma.Imagen = string.Concat(path, ma.Imagen);
                    }
                });
            }

            return Ok(dataMarcaauto);
        }       
    }
}
