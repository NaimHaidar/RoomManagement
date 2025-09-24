using RoomManagement.Repository.Models;

public class NewAttendeeDto
{
    
    public string UserId { get; set; }
    public NewAttendeeDto() { }
    public NewAttendeeDto(Attendee a) {
        UserId = a.UserId;
    }





}
