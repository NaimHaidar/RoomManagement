using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("Attachment")]
public partial class Attachment
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Directory { get; set; }

    public int MeetingId { get; set; }

    [ForeignKey("MeetingId")]
    [InverseProperty("Attachments")]
    public virtual Meeting Meeting { get; set; } = null!;
}
