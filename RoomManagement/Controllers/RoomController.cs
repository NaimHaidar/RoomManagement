using Microsoft.AspNetCore.Mvc;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
    

        [HttpGet(Name = "GetRoom")]
        public IEnumerable<Room> Get()
        { using (var context = new RoomManagementDBContext())
            {
                return context.Rooms.ToList();
            }
        }

        [HttpGet("{id}")]
        public IEnumerable<Room> GetById(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
               
                return context.Rooms.Where(r => r.Id == id).ToList();
            }
        }

        [HttpPost(Name ="setRoom")]
        public IEnumerable<Room> set(string name,int capacity,int locationId)
        {
            using (var context = new RoomManagementDBContext())
            {Room room = new Room(name,capacity,locationId);
                context.Rooms.Add(room);
                context.SaveChanges();
                return context.Rooms.Where(r => r.Id == room.Id).ToList();
            }
        }
        [HttpPut(Name = "UpdateRoom")]
        public IEnumerable<Room> Update(int id ,string name,int capacity,int locationId)
        {
            using (var context = new RoomManagementDBContext())
            {
                var room = context.Rooms.Where(r => r.Id == id).FirstOrDefault();
                if (room != null)
                {
                    room.Name = name;
                    room.Capacity = capacity;
                    room.LocationId = locationId;
                    context.SaveChanges(); 
                }
               
               
                return context.Rooms.Where(r => r.Id == id).ToList();
            }
        }
        [HttpDelete( Name = "DeleteRoom")]
        public IActionResult Delete(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
                var room = context.Rooms.FirstOrDefault(u => u.Id == id);
                if (room == null)
                {
                    return NotFound(new { message = $"Room with ID {id} not found." });
                }
        
                context.Rooms.Remove(room);
                context.SaveChanges();
                return Ok(new { message = $"Room with ID {id} has been deleted." });
            }
        }
        

    }
}
