using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("RoomFeature")]
public partial class RoomFeature
{
    [Key]
    public int Id { get; set; }

    public int FeatureId { get; set; }

    public int RoomId { get; set; }

    [ForeignKey("FeatureId")]
    [InverseProperty("RoomFeatures")]
    public virtual Feature Feature { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("RoomFeatures")]
    public virtual Room Room { get; set; } = null!;
}
