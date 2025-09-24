public class MinutesOfMeetingDto
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Body { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public string? UserId { get; set; }
    public int MeetingId { get; set; }

    public MinutesOfMeetingDto() { }

    public MinutesOfMeetingDto(RoomManagement.Repository.Models.MinutesOfMeeting mom)
    {
        Id = mom.Id;
        Type = mom.Type;
        Body = mom.Body;
        CreationDate = mom.CreationDate;
        DeadlineDate = mom.DeadlineDate;
        UserId = mom.UserId;
        MeetingId = mom.MeetingId;
    }
}