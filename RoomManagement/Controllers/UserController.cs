using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
    

        [HttpGet(Name = "GetUser")]
        public IEnumerable<UserDto> Get()
        { using (var context = new RoomManagementDBContext())
            {
                return context.Users.Select(u => new UserDto(u.Id, u.Name, u.Email, u.RoleId)).ToList();
            }
        }

        [HttpGet("{id}")]
        public UserDto GetById(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
               
                return context.Users.Where(u => u.Id == id).Select(u=>new UserDto(u)).First();
            }
        }

        [HttpPost(Name ="setUser")]
        public NewUserDto set(NewUserDto u)
        {
            using (var context = new RoomManagementDBContext())
            {User user = new User(u.Name, u.Email,u.Password,u.RoleId);
                context.Users.Add(user);
                context.SaveChanges();
                return context.Users.Where(u => u.Id == user.Id).Select(u => new NewUserDto(u)).First();
            }
        }
        [HttpPut(Name = "UpdateUser")]
        public NewUserDto Update(int id ,string name, string email, string pass, int roleId)
        {
            using (var context = new RoomManagementDBContext())
            {
                var user = context.Users.Where(u => u.Id == id).First();
                if (user != null)
                {
                    user.Name = name;
                    user.Email = email;
                    user.Password = pass;
                    user.RoleId = roleId;
                    context.SaveChanges();
                }
                context.SaveChanges();
                return context.Users.Where(u => u.Id == id).Select(u=>new NewUserDto(u)).First();
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
