using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Cwiczenie5.Data.Models;
using Cwiczenie5.DTOs;

namespace Cwiczenie5.Controllers
{
    [Route("api")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly YourDbContext _context;

        public TripController(YourDbContext context)
        {
            _context = context;
        }

        [HttpGet("trips")]
        public ActionResult<IEnumerable<TripDTO>> GetTrips()
        {
            var trips = _context.Trip
                .OrderByDescending(t => t.DateFrom)
                .Select(t => new TripDTO
                {
                    Name = t.Name,
                    Description = t.Description,
                    DateFrom = t.DateFrom,
                    DateTo = t.DateTo,
                    MaxPeople = t.MaxPeople,
                    Countries = _context.Country_Trip
                        .Where(ct => ct.IdTrip == t.IdTrip)
                        .Select(ct => new CountryDTO { Name = ct.Country.Name })
                        .ToList(),
                    Clients = _context.Client_Trip
                        .Where(ct => ct.IdTrip == t.IdTrip)
                        .Select(ct => new ClientDTO
                        {
                            FirstName = ct.Client.FirstName,
                            LastName = ct.Client.LastName,
                            Email = ct.Client.Email,
                            Telephone = ct.Client.Telephone,
                            Pesel = ct.Client.Pesel
                        })
                        .ToList()
                })
                .ToList();

            return Ok(trips);
        }

        [HttpDelete("clients/{idClient}")]
        public IActionResult DeleteClient(int idClient)
        {
            var client = _context.Client.Find(idClient);
            if (client == null)
                return NotFound("Client not found.");

            var hasTrips = _context.Client_Trip.Any(ct => ct.IdClient == idClient);
            if (hasTrips)
                return Conflict("Client has trips assigned and cannot be deleted.");

            _context.Client.Remove(client);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("trips/{idTrip}/clients")]
        public IActionResult AddClientToTrip(int idTrip, ClientTripDTO clientTripDTO)
        {
            var trip = _context.Trip.Find(idTrip);
            if (trip == null)
                return NotFound("Trip not found.");

            var existingClient = _context.Client.FirstOrDefault(c => c.Pesel == clientTripDTO.Pesel);
            if (existingClient == null)
            {
                existingClient = new Client
                {
                    FirstName = clientTripDTO.FirstName,
                    LastName = clientTripDTO.LastName,
                    Email = clientTripDTO.Email,
                    Telephone = clientTripDTO.Telephone,
                    Pesel = clientTripDTO.Pesel
                };
                _context.Client.Add(existingClient);
                _context.SaveChanges();
            }

            var existingClientTrip = _context.Client_Trip.FirstOrDefault(ct => ct.IdTrip == idTrip && ct.IdClient == existingClient.IdClient);
            if (existingClientTrip != null)
                return Conflict("Client is already assigned to this trip.");

            var clientTrip = new Client_Trip
            {
                IdClient = existingClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientTripDTO.PaymentDate
            };
            _context.Client_Trip.Add(clientTrip);
            _context.SaveChanges();

            return Ok();
        }
    }
}