using Microsoft.AspNetCore.Mvc;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
    

        [HttpGet(Name = "GetRole")]
        public IEnumerable<Role> Get()
        { using (var context = new RoomManagementDBContext())
            {
                return context.Roles.ToList();
            }
        }

        [HttpGet("{id}")]
        public IEnumerable<Role> GetById(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
               
                return context.Roles.Where(r => r.Id == id).ToList();
            }
        }

        [HttpPost(Name ="setRole")]
        public IEnumerable<Role> set(string name)
        {
            using (var context = new RoomManagementDBContext())
            {Role role = new Role(name);
                context.Roles.Add(role);
                context.SaveChanges();
                return context.Roles.Where(r => r.Id == role.Id).ToList();
            }
        }
        [HttpPut(Name = "UpdateRole")]
        public IEnumerable<Role> Update(int id ,string name)
        {
            using (var context = new RoomManagementDBContext())
            {
                var role = context.Roles.Where(r => r.Id == id).FirstOrDefault();
                if (role != null)
                {
                    role.role = name;
                    context.SaveChanges(); 
                }
               
               
                return context.Roles.Where(r => r.Id == id).ToList();
            }
        }
        [HttpDelete( Name = "DeleteRole")]
        public IActionResult Delete(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
                var role = context.Roles.FirstOrDefault(u => u.Id == id);
                if (role == null)
                {
                    return NotFound(new { message = $"Role with ID {id} not found." });
                }

                context.Roles.Remove(role);
                context.SaveChanges();
                return Ok(new { message = $"Role with ID {id} has been deleted." });
            }
        }


    }
}
