
using RoomManagement.Repository.DTOs;
using RoomManagement.Repository.Models;

public class NewMeetingDto
{
    public string? Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string UserId { get; set; } = null!;
    public int RoomId { get; set; }
    public ICollection<NewAttendeeDto> attendees { get; set; } = new List<NewAttendeeDto>();

    public NewMeetingDto() { }
    public NewMeetingDto(Meeting meeting)
    {
        Title = meeting.Title;
        StartDate = meeting.StartDate;
        EndDate = meeting.EndDate;
        UserId = meeting.UserId;
        RoomId = meeting.RoomId;
        foreach (var att in meeting.Attendees)
        {
            attendees.Add(new NewAttendeeDto(att));
        }

    }
}