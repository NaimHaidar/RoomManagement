
using RoomManagement.Repository.Models;

public class NewUserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
        public string Password { get; set; } = null!;
    public NewUserDto() { }
        public NewUserDto(int id, string name, string email, int roleId ,string pass)
        {
            Id = id;
            Name = name;
            Email = email;
            RoleId = roleId;
            Password = pass;
    }
        public NewUserDto(User user)
    {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            RoleId = user.RoleId;
            Password = user.Password;
    }
    }
