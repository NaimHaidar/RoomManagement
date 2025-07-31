using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("Room")]
public partial class Room
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Name { get; set; }

    public int Capacity { get; set; }

    public int LocationId { get; set; }

    [ForeignKey("LocationId")]
    [InverseProperty("Rooms")]
    public virtual Location Location { get; set; } = null!;

    [InverseProperty("Room")]
    public virtual ICollection<Meeting> Meetings { get; set; }
    [InverseProperty("Room")]
    public virtual ICollection<RoomFeature> RoomFeatures { get; set; } 
    public Room()
    {
        Meetings = new List<Meeting>();
        RoomFeatures = new List<RoomFeature>();
    }
    public Room(string name, int capacity, int locationId)
    {
        Name = name;
        Capacity = capacity;
        LocationId = locationId;
        Meetings = new List<Meeting>();
        RoomFeatures = new List<RoomFeature>();
    }
}
