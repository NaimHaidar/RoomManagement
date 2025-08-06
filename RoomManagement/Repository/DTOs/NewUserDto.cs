using RoomManagement.Repository.Models;
using System.ComponentModel.DataAnnotations;

public class NewUserDto
{
   

    [StringLength(30)]
    public string? Name { get; set; } 

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public String Role { get; set; } = "gest"; 

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters")]
    public string Password { get; set; } = null!;

    public NewUserDto() { }
  

    

}
