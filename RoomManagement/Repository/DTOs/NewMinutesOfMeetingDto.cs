public class NewMinutesOfMeetingDto
{
    public string? Type { get; set; }
    public string? Body { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public string? UserId { get; set; }
    public int MeetingId { get; set; }

    public NewMinutesOfMeetingDto() { }
}