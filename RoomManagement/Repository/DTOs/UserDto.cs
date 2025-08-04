
using RoomManagement.Repository.Models;

public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
    public  UserDto() { }
        public UserDto(int id, string name, string email, int roleId )
        {
            Id = id;
            Name = name;
            Email = email;
            RoleId = roleId;
    }
        public UserDto(User user)
    {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            RoleId = user.RoleId;
         
    }
    }
