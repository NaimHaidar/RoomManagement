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

   
    public string Password { get; set; } = null!;

    public NewUserDto() { }
  

    

}
