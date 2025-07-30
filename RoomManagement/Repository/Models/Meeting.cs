using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("Meeting")]
public partial class Meeting
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Title { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    [InverseProperty("Meeting")]
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    [InverseProperty("Meeting")]
    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    [InverseProperty("Meeting")]
    public virtual ICollection<MinutesOfMeeting> MinutesOfMeetings { get; set; } = new List<MinutesOfMeeting>();

    [ForeignKey("RoomId")]
    [InverseProperty("Meetings")]
    public virtual Room Room { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Meetings")]
    public virtual User User { get; set; } = null!;
}
