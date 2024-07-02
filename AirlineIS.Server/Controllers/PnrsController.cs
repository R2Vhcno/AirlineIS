using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineIS.Server.Models;

namespace AirlineIS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PnrsController : ControllerBase
    {
        private readonly ArsContext _context;

        public PnrsController(ArsContext context)
        {
            _context = context;
        }

        // GET: api/Pnrs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PnrView>>> GetPnrs()
        {
            return await _context.PnrViews.ToListAsync();
        }
    }
}
