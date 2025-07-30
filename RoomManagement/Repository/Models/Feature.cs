using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("Feature")]
public partial class Feature
{
    [Key]
    public int Id { get; set; }

    [Column("Feature")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Feature1 { get; set; }

    [InverseProperty("Feature")]
    public virtual ICollection<RoomFeature> RoomFeatures { get; set; } = new List<RoomFeature>();
}
