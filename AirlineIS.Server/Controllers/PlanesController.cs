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
    public class PlanesController : ControllerBase
    {
        private readonly ArsContext _context;

        public PlanesController(ArsContext context)
        {
            _context = context;
        }

        // GET: api/Planes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plane>>> GetPlanes()
        {
            return await _context.Planes.ToListAsync();
        }

        // GET: api/Planes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plane>> GetPlane(int id)
        {
            var plane = await _context.Planes.FindAsync(id);

            if (plane == null)
            {
                return NotFound();
            }

            return plane;
        }

        // GET: api/Planes/CanDelete/5
        [HttpGet("CanDelete/{id}")]
        public async Task<ActionResult<bool>> CanDelete(int id) {
            return !(await _context.Flights.AnyAsync(flight => flight.PlaneId == id));
        }

        // PUT: api/Planes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlane(int id, Plane plane)
        {
            if (id != plane.Id)
            {
                return BadRequest();
            }

            _context.Entry(plane).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Planes
        [HttpPost]
        public async Task<ActionResult<Plane>> PostPlane(Plane plane)
        {
            _context.Planes.Add(plane);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlane", new { id = plane.Id }, plane);
        }

        // DELETE: api/Planes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlane(int id)
        {
            var plane = await _context.Planes.FindAsync(id);
            if (plane == null)
            {
                return NotFound();
            }

            // Remove all associated seats beforehand
            var seats = await _context.PlaneSeats.Where(seat => seat.PlaneId == id).ToListAsync();

            foreach (var seat in seats) {
                _context.Remove(seat);
            }

            _context.Planes.Remove(plane);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaneExists(int id)
        {
            return _context.Planes.Any(e => e.Id == id);
        }
    }
}
