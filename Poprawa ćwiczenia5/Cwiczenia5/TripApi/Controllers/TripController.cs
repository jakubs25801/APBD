using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripApi.Data;
using TripApi.DTOs;
using TripApi.Model;

namespace TripApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly TripContext _context;

    public TripsController(TripContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
    {
        return await _context.Trips
            .OrderByDescending(t => t.StartDate)
            .ToListAsync();
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientTripDto dto)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound("Trip not found.");
        }

        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.PESEL == dto.PESEL);

        if (client == null)
        {
            client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PESEL = dto.PESEL
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        if (client.ClientTrips.Any(ct => ct.TripId == idTrip))
        {
            return BadRequest("Client is already assigned to this trip.");
        }

        var clientTrip = new ClientTrip
        {
            ClientId = client.Id,
            TripId = idTrip,
            PaymentDate = dto.PaymentDate,
            RegisteredAt = DateTime.UtcNow
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return Ok();
    }
}