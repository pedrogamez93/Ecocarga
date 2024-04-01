using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EcocargaApi.Controllers
{   /// <summary>
    /// Controller de versiones
    /// </summary>
    [Route("api/versiones")]
    [Produces("application/json")]
    [ApiController]
    public class VersionesController : ControllerBase
    {
        private readonly EcocargaContext _context;

        /// <summary>
        /// Controller de versiones
        /// </summary>
        public VersionesController(EcocargaContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Obtiene todas las versiones
        /// </summary>
        /// <returns></returns>
        // GET: api/versiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataVersion>>> GetDataVersion()
        {
            var dataVersion = await _context.DataVersion.ToListAsync();
            return Ok(dataVersion);
        }
        /// <summary>
        /// Obtiene versión por id
        /// </summary>
        /// <param name="id">Id de versión</param>
        /// <returns>uhuyvhvu</returns>
        // GET: api/versiones/5
        [HttpGet("{id}", Name = "idVersion")]
        public async Task<ActionResult<DataVersion>> GetDataVersion([BindRequired]int id)
        {
            var dataVersion = await _context.DataVersion.FindAsync(id);

            if (dataVersion == null)
            {
                return NotFound();
            }

            return Ok(dataVersion);
        }
        /// <summary>
        /// Obtiene la ultima versión
        /// </summary>
        /// <returns></returns>
        // GET: api/versiones/ultima-version
        [HttpGet("ultima-version")]
        public async Task<ActionResult<DataVersion>> GetLastDataVersion()
        {
            var dataVersion = await _context.DataVersion.OrderByDescending(v => v.Timestamp).FirstAsync();

            if (dataVersion == null)
            {
                return NotFound();
            }

            return Ok(dataVersion);
        }
    }
}
