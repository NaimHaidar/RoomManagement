using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
    

        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        { using (var context = new RoomManagementDBContext())
            {
                return context.Users.ToList();
            }
        }

        [HttpGet("{id}")]
        public IEnumerable<User> GetById(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
               
                return context.Users.Where(u => u.Id == id).ToList();
            }
        }

        [HttpPost(Name ="setUser")]
        public IEnumerable<User> set(string name,string email,string pass,int roleId)
        {
            using (var context = new RoomManagementDBContext())
            {User user = new User(name, email, pass, roleId);
                context.Users.Add(user);
                context.SaveChanges();
                return context.Users.Where(u => u.Id == user.Id).ToList();
            }
        }
        [HttpPut(Name = "UpdateUser")]
        public IEnumerable<User> Update(int id ,string name, string email, string pass, int roleId)
        {
            using (var context = new RoomManagementDBContext())
            {
                var user = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (user != null)
                {
                    user.Name = name;
                    user.Email = email;
                    user.Password = pass;
                    user.RoleId = roleId;
                    context.SaveChanges();
                }
                context.SaveChanges();
                return context.Users.Where(u => u.Id == id).ToList();
            }
        }
        [HttpDelete( Name = "DeleteUser")]
        public IActionResult Delete(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                context.Users.Remove(user);
                context.SaveChanges();
                return Ok(new { message = $"User with ID {id} has been deleted." });
            }
        }


    }
}
