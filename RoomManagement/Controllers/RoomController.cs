using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;
using Microsoft.AspNetCore.Authorization; 

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
        [AllowAnonymous] 
        public IEnumerable<Room> Get()
        {
            return _context.Rooms.ToList();
        }

        [HttpGet("{id}")]
        [AllowAnonymous] 
        public IEnumerable<Room> GetById(int id)
        {
            return _context.Rooms.Where(r => r.Id == id).ToList();
        }

        [HttpPost(Name = "setRoom")]
        [Authorize(Roles = "Admin")] 
        public IEnumerable<Room> Set(string name, int capacity, int locationId)
        {
            Room room = new Room(name, capacity, locationId);
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return _context.Rooms.Where(r => r.Id == room.Id).ToList();
        }

        [HttpPut(Name = "UpdateRoom")]
        [Authorize(Roles = "Admin")] 
        public IEnumerable<Room> Update(int id, string name, int capacity, int locationId)
        {
            var room = _context.Rooms.Where(r => r.Id == id).FirstOrDefault();
            if (room != null)
            {
                room.Name = name;
                room.Capacity = capacity;
                room.LocationId = locationId;
                _context.SaveChanges();
            }
            return _context.Rooms.Where(r => r.Id == id).ToList();
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
    }
}
