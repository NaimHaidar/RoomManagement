using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using RoomManagement.Repository.DTOs;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] 
    public class RoomController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public RoomController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetRoom")]
        [Authorize]
        public async Task<IEnumerable<RoomDto>> Get()
        {
            return await _context.Rooms.Select(r => new RoomDto(r)).ToListAsync();
        }

        [HttpGet("count", Name = "GetNbrRoom")]
        public async Task<int> GetNbr()
        {
            return await _context.Rooms.CountAsync();
        }
        [HttpGet("{id}")]
        [Authorize] 
        public async Task<IEnumerable<RoomDto>> GetById(int id)
        {
            return await _context.Rooms.Where(r => r.Id == id).Select(r=>new RoomDto(r)).ToListAsync();
        }

        [HttpPost(Name = "setRoom")]
        [Authorize(Roles = "Admin")] 
        public async Task<IEnumerable<RoomDto>> Set(NewRoomDto Room)
        {
            Room room = new Room(Room);
            _context.Rooms.Add(room);
             await _context.SaveChangesAsync();
            return _context.Rooms.Where(r => r.Id == room.Id).Select(r=>new RoomDto(r)).ToList();
        }

        [HttpPut(Name = "UpdateRoom")]
        [Authorize(Roles = "Admin")] 
      public async Task<IEnumerable<RoomDto>> Update(RoomDto Room)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == Room.Id);
            if (room != null)
            {
                room.Name = Room.Name;
                room.Capacity = Room.Capacity;
                room.LocationId = Room.LocationId;
                await _context.SaveChangesAsync();
            }
            return _context.Rooms.Where(r => r.Id == Room.Id).Select(r => new RoomDto(r)).ToList();
        }

        [HttpDelete(Name = "DeleteRoom")]
        [Authorize(Roles = "Admin")] 
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(u => u.Id == id);
            if (room == null)
            {
                return NotFound(new { message = $"Room with ID {id} not found." });
            }

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Ok(new { message = $"Room with ID {id} has been deleted." });
        }

        [HttpGet("{roomId}/features")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FeatureDto>>> GetRoomFeatures(int roomId)
        {
            var roomExists = await _context.Rooms.AnyAsync(r => r.Id == roomId);
            if (!roomExists)
                return NotFound(new { message = $"Room with ID {roomId} not found." });

            
            var features = await _context.RoomFeatures
                .Where(rf => rf.RoomId == roomId)
                .Join(
                    _context.Features,             
                    rf => rf.FeatureId,            
                    f => f.Id,                     
                    (rf, f) => new FeatureDto (f)     
                    
                )
                .ToListAsync();

            return Ok(features);
        }

    }
}
