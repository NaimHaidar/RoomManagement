using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository.DTOs;

namespace RoomManagement.Repository.Models;

[Table("Attendee")]
public partial class Attendee
{
    [Key]
    public int Id { get; set; }

    public int MeetingId { get; set; }

    public string UserId { get; set; } = null!;

    [ForeignKey("MeetingId")]
    [InverseProperty("Attendees")]
    public virtual Meeting Meeting { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Attendees")]
    public virtual User User { get; set; } = null!;
    public Attendee() { }
    public Attendee(AttendeeDto attendee)
    {
        Id = attendee.Id;
        MeetingId = attendee.MeetingId;
        UserId = attendee.UserId;
    }
    public Attendee(NewAttendeeDto attendee,int meetingId)
    {
        MeetingId = meetingId;
        UserId = attendee.UserId;
    }
}
