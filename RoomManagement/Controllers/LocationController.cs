using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;
using RoomManagement.Repository.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public LocationController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetLocation")]
        [Authorize]
        public async Task<IEnumerable<LocationDto>> Get()
        {
            return await _context.Locations.Select(l => new LocationDto(l)).ToListAsync();
        }

        [HttpGet("count", Name = "GetNbrLocation")]
        public async Task<int> GetNbr()
        {
            return await _context.Locations.CountAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IEnumerable<LocationDto>> GetById(int id)
        {
            return await _context.Locations.Where(l => l.Id == id).Select(l => new LocationDto(l)).ToListAsync();
        }

        [HttpPost(Name = "setLocation")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<LocationDto>> Set(NewLocationDto locationDto)
        {
            Location location = new Location(locationDto);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return _context.Locations.Where(l => l.Id == location.Id).Select(l => new LocationDto(l)).ToList();
        }

        [HttpPut(Name = "UpdateLocation")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<LocationDto>> Update(LocationDto locationDto)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == locationDto.Id);
            if (location != null)
            {
                location.Country = locationDto.Country;
                location.City = locationDto.City;
                location.Building = locationDto.Building;
                location.Floor = locationDto.Floor;
                location.MapLink = locationDto.MapLink;
                await _context.SaveChangesAsync();
            }
            return _context.Locations.Where(l => l.Id == locationDto.Id).Select(l => new LocationDto(l)).ToList();
        }

        [HttpDelete(Name = "DeleteLocation")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == id);
            if (location == null)
            {
                return NotFound(new { message = $"Location with ID {id} not found." });
            }

            _context.Locations.Remove(location);
            _context.SaveChanges();
            return Ok(new { message = $"Location with ID {id} has been deleted." });
        }
    }
}