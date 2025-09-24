
using RoomManagement.Repository.DTOs;
using RoomManagement.Repository.Models;

public class MeetingDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
    public ICollection<AttendeeDto> attendees { get; set; } = new List<AttendeeDto>();

    public MeetingDto() { }
    public MeetingDto(Meeting meeting)
    {
        Id = meeting.Id;
        Title = meeting.Title;
        StartDate = meeting.StartDate;
        EndDate = meeting.EndDate;
        UserId = meeting.UserId;
        RoomId = meeting.RoomId;
        foreach (var att in meeting.Attendees)
        {
            attendees.Add(new AttendeeDto(att));
        }

    }
}