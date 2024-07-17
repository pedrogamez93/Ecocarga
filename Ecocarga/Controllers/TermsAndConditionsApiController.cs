using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecocarga.Data;
using Ecocarga.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecocarga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermsAndConditionsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TermsAndConditionsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TermsAndConditionsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TermsAndConditions>>> GetTermsAndConditions()
        {
            return await _context.TermsAndConditions.ToListAsync();
        }

        // GET: api/TermsAndConditionsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TermsAndConditions>> GetTermsAndConditions(int id)
        {
            var termsAndConditions = await _context.TermsAndConditions.FindAsync(id);

            if (termsAndConditions == null)
            {
                return NotFound();
            }

            return termsAndConditions;
        }

        // Additional methods (POST, PUT, DELETE) can be added here if needed
    }
}
