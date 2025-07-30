using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D1053463D78678", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Name { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Password { get; set; } = null!;
    [ForeignKey("RoleId")]
    public int RoleId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Attendee> Attendees { get; set; } 

    [InverseProperty("User")]
    public virtual ICollection<Meeting> Meetings { get; set; } 

    [InverseProperty("User")]
    public virtual ICollection<MinutesOfMeeting> MinutesOfMeetings { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; }

   
    //[InverseProperty("Users")]
    //public virtual Role Role { get; set; } = null!;
    public User()
    {
        Attendees = new List<Attendee>();
        Meetings = new List<Meeting>();
        MinutesOfMeetings = new List<MinutesOfMeeting>();
        Notifications = new List<Notification>();
    }
    public User( string name, string email, string password, int roleId)
    {
    Name = name;
    Email = email;
    Password = password;
    RoleId = roleId;
    Attendees = new List<Attendee>();
    Meetings = new List<Meeting>();
    MinutesOfMeetings = new List<MinutesOfMeeting>();
    Notifications = new List<Notification>();
    }
   
}
