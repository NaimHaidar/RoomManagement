using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("Location")]
public partial class Location
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Country { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? City { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Building { get; set; }

    public int Floor { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? MapLink { get; set; }

    [InverseProperty("Location")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    public Location() { }
    public Location(NewLocationDto location)
    {
        Country = location.Country;
        City = location.City;
        Building = location.Building;
        Floor = location.Floor;
        MapLink = location.MapLink;
    }
}
