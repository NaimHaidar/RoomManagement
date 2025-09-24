using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.DTOs;
using RoomManagement.Repository.Models;

namespace RoomManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize] 
    public class MeetingController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public MeetingController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetMeeting")]
        public async Task<IEnumerable<MeetingDto>> Get()
        {
            var meetings = await _context.Meetings.ToListAsync();
            var meetingDtos = new List<MeetingDto>();
            foreach (var meeting in meetings)
            {
                meetingDtos.Add(new MeetingDto(meeting));
            }
            return meetingDtos;
        }
        [HttpGet("count", Name = "GetNbrMeeting")]
        public async Task<int> GetNbr()
        {
            return await _context.Meetings.CountAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingDto>> GetById(int id)
        {
            var meeting = await _context.Meetings
                .FirstOrDefaultAsync(m=> m.Id == id);

            if (meeting == null)
            {
                return NotFound();
            }

            MeetingDto meetingDto = new(meeting);
            return Ok(meetingDto);
        }

        [HttpPost(Name = "SetMeeting")]
        public async Task<ActionResult<MeetingDto>> PostMeeting(NewMeetingDto meetingDto)
        {
           if (meetingDto == null) { 
                return BadRequest("Meeting data is null.");
            }
            if (!IsValidMeeting(meetingDto.StartDate,meetingDto.EndDate,meetingDto.RoomId))
            {
                return BadRequest("scheduling conflict.");
            }
            var meeting = new Meeting(meetingDto);

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return new MeetingDto(meeting);
        }
        private  bool IsValidMeeting(DateTime startDate, DateTime endDate, int roomId,int? meetingId=null)
        {
            if (startDate >= endDate)
                return false;

            if (startDate < DateTime.Now || endDate < DateTime.Now)
                return false;

            
           
          

            bool roomConflict =  _context.Meetings.Any(m =>
               m.Id!=meetingId&& m.RoomId == roomId &&
                (
                    (startDate >= m.StartDate && startDate < m.EndDate) ||   (m.StartDate>=startDate && m.StartDate<endDate)     
                )
            );

            

            return !roomConflict;
        }



        [HttpPut(Name ="UpdateMeeting")]
        public async Task<IActionResult> PutMeeting( MeetingDto meeting)
        {
            var existingMeeting = await _context.Meetings.FindAsync(meeting.Id);
            if (existingMeeting == null)
            {
                return NotFound();
            }
            if (!IsValidMeeting(meeting.StartDate, meeting.EndDate, meeting.RoomId,meeting.Id))
            {
                return BadRequest("scheduling conflict.");
            }

            existingMeeting.Title = meeting.Title;
            existingMeeting.StartDate = meeting.StartDate;
            existingMeeting.EndDate = meeting.EndDate;
            existingMeeting.UserId = meeting.UserId;
            existingMeeting.RoomId = meeting.RoomId;

            await _context.SaveChangesAsync();

            return Ok(new MeetingDto(existingMeeting));
        }

    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

      
        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }

        [HttpPost("{meetingId}/attendees")]
        [Authorize]
        public async Task<IActionResult> AddAttendees(int meetingId, [FromBody] List<NewAttendeeDto> attendees)
        {
            var meeting = await _context.Meetings.Include(m => m.Attendees).FirstOrDefaultAsync(m => m.Id == meetingId);
            if (meeting == null)
            {
                return NotFound(new { message = $"Meeting with ID {meetingId} not found." });
            }

            foreach (var attendeeDto in attendees)
            {
                
                if (!meeting.Attendees.Any(a => a.UserId == attendeeDto.UserId))
                {
                    var attendee = new Attendee(attendeeDto,meetingId);
                    attendee.MeetingId = meetingId;
                    _context.Attendees.Add(attendee);
                }
            }

            await _context.SaveChangesAsync();
            var result = await _context.Attendees.Where(a => a.MeetingId == meetingId).Select(a => new AttendeeDto(a)).ToListAsync();
            return Ok(result);
        }
    }
}
