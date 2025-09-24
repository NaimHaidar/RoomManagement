using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace RoomManagement.Repository.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Unicode(false)]
        public string? Name { get; set; }

        public int Capacity { get; set; }

        public int LocationId { get; set; }
        public RoomDto() { }
        public RoomDto(Room room)
        {
            Id = room.Id;
            Name = room.Name;
            Capacity = room.Capacity;
            LocationId = room.LocationId;
        }
    }
}
