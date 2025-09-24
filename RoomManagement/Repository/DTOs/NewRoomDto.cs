using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace RoomManagement.Repository.DTOs
{
    public class NewRoomDto
    {

        [StringLength(30)]
        [Unicode(false)]
        public string? Name { get; set; }

        public int Capacity { get; set; }

        public int LocationId { get; set; }
        public NewRoomDto() { }
        public NewRoomDto(Room room)
        {
            Name = room.Name;
            Capacity = room.Capacity;
            LocationId = room.LocationId;
        }
    }
}
