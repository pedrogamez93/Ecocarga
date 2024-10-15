using Ecocarga.Data;
using Ecocarga.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Ecocarga.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BateriasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BateriasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BateriasApi
        [HttpGet]
        public ActionResult<IEnumerable<Bateria>> GetBaterias()
        {
            var baterias = _context.Baterias.ToList();
            return Ok(baterias);
        }
    }
}
