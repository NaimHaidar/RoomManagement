using RoomManagement.Repository.Models;

public class UserDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; } = null!;
    public IList<string> Roles { get; set; }

  

    public UserDto(User user, IList<string> roles)
    {
        Id = user.Id; 
        Name = user.Name;
        Email = user.Email;
        Roles = roles;
    }
}
