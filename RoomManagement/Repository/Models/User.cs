using Microsoft.AspNetCore.Identity; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models
{
    [Table("User")]
    public partial class User : IdentityUser 
    {
        [StringLength(30)]
        [Unicode(false)]
        public string? Name { get; set; } 
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

        [InverseProperty("User")]
        public virtual ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();

        [InverseProperty("User")]
        public virtual ICollection<MinutesOfMeeting> MinutesOfMeetings { get; set; } = new List<MinutesOfMeeting>();

        [InverseProperty("User")]
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public User()
        {
            Attendees = new List<Attendee>();
            Meetings = new List<Meeting>();
            MinutesOfMeetings = new List<MinutesOfMeeting>();
            Notifications = new List<Notification>();
        }

        public User(string name, string email) : base()
        {
            Name = name;
            this.Email = email;       
            this.UserName = email; 
            Attendees = new List<Attendee>();
            Meetings = new List<Meeting>();
            MinutesOfMeetings = new List<MinutesOfMeeting>();
            Notifications = new List<Notification>();
        }
    }
}
