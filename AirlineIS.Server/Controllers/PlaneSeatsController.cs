using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineIS.Server.Models;

namespace AirlineIS.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PlaneSeatsController : ControllerBase {
        private readonly ArsContext _context;

        public PlaneSeatsController(ArsContext context) {
            _context = context;
        }

        // GET: api/PlaneSeats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaneSeat>>> GetPlaneSeats() {
            return await _context.PlaneSeats.Include(x => x.Class).ToListAsync();
        }

        // GET: api/PlaneSeats/available/5
        [HttpGet("available/{flightId}")]
        public async Task<ActionResult<IEnumerable<PlaneSeat>>> GetAvailablePlaneSeats(int flightId) {
            var flight = await _context.Flights.FindAsync(flightId);

            if (flight == null) {
                return NotFound();
            }

            var flightTickets = await _context.Tickets.Where(ticket => ticket.FlightId == flight.Id).ToListAsync();

            var flightSeats = await _context.PlaneSeats.Where(seat => seat.PlaneId == flight.PlaneId).Include(seat => seat.Class).ToListAsync();

            return flightSeats.Where(seat => !flightTickets.Any(ticket => ticket.PlaneSeatId == seat.Id)).ToList();
        }

        // GET: api/PlaneSeats/plane/5
        [HttpGet("plane/{planeId}")]
        public async Task<ActionResult<IEnumerable<PlaneSeat>>> GetPlaneSeats(int planeId) {
            var plane = await _context.Planes.FindAsync(planeId);

            if (plane == null) {
                return NotFound();
            }

            return await _context.PlaneSeats.Where(seat => seat.PlaneId == planeId).Include(seat => seat.Class).ToListAsync();
        }

        // GET: api/PlaneSeats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaneSeat>> GetPlaneSeat(int id) {
            var planeSeat = await _context.PlaneSeats.FindAsync(id);

            if (planeSeat == null) {
                return NotFound();
            }

            planeSeat.Class = await _context.Classes.FindAsync(planeSeat.ClassId);

            return planeSeat;
        }

        // PUT: api/PlaneSeats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaneSeat(int id, PlaneSeat planeSeat) {
            if (id != planeSeat.Id) {
                return BadRequest();
            }

            _context.Entry(planeSeat).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!PlaneSeatExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PlaneSeats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlaneSeat>> PostPlaneSeat(PlaneSeat planeSeat) {
            _context.PlaneSeats.Add(planeSeat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaneSeat", new { id = planeSeat.Id }, planeSeat);
        }

        // DELETE: api/PlaneSeats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaneSeat(int id) {
            var planeSeat = await _context.PlaneSeats.FindAsync(id);
            if (planeSeat == null) {
                return NotFound();
            }

            _context.PlaneSeats.Remove(planeSeat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlaneSeatExists(int id) {
            return _context.PlaneSeats.Any(e => e.Id == id);
        }
    }
}
