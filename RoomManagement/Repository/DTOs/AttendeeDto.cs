using RoomManagement.Repository.Models;

namespace RoomManagement.Repository.DTOs
{
    public class AttendeeDto
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public int MeetingId { get; set;}
        public AttendeeDto() { }
        public AttendeeDto(Attendee attendee)
        {
            Id = attendee.Id;
            UserId = attendee.UserId;
            MeetingId = attendee.MeetingId;
        }
        
    }
}