using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("MinutesOfMeeting")]
public partial class MinutesOfMeeting
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Type { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Body { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeadlineDate { get; set; }

    public string? UserId { get; set; }

    public int MeetingId { get; set; }

    [ForeignKey("MeetingId")]
    [InverseProperty("MinutesOfMeetings")]
    public virtual Meeting Meeting { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("MinutesOfMeetings")]
    public virtual User? User { get; set; }
}
