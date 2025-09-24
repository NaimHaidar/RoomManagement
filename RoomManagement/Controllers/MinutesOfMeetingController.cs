using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;
using RoomManagement.Repository.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MinutesOfMeetingController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public MinutesOfMeetingController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<MinutesOfMeetingDto>> Get()
        {
            return await _context.MinutesOfMeetings
                .Select(m => new MinutesOfMeetingDto(m))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MinutesOfMeetingDto>> GetById(int id)
        {
            var mom = await _context.MinutesOfMeetings.FindAsync(id);
            if (mom == null)
                return NotFound();
            return new MinutesOfMeetingDto(mom);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<MinutesOfMeetingDto>> Post(NewMinutesOfMeetingDto dto)
        {
            var mom = new MinutesOfMeeting
            {
                Type = dto.Type,
                Body = dto.Body,
                CreationDate = dto.CreationDate,
                DeadlineDate = dto.DeadlineDate,
                UserId = dto.UserId,
                MeetingId = dto.MeetingId
            };
            _context.MinutesOfMeetings.Add(mom);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = mom.Id }, new MinutesOfMeetingDto(mom));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Put(int id, [FromBody] NewMinutesOfMeetingDto dto)
        {
            var mom = await _context.MinutesOfMeetings.FindAsync(id);
            if (mom == null)
                return NotFound();

            mom.Type = dto.Type;
            mom.Body = dto.Body;
            mom.CreationDate = dto.CreationDate;
            mom.DeadlineDate = dto.DeadlineDate;
            mom.UserId = dto.UserId;
            mom.MeetingId = dto.MeetingId;

            await _context.SaveChangesAsync();
            return Ok(new MinutesOfMeetingDto(mom));
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var mom = await _context.MinutesOfMeetings.FindAsync(id);
            if (mom == null)
                return NotFound();

            _context.MinutesOfMeetings.Remove(mom);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"MinutesOfMeeting with ID {id} has been deleted." });
        }

        [HttpGet("by-meeting/{meetingId}")]
        public async Task<ActionResult<IEnumerable<MinutesOfMeetingDto>>> GetByMeetingId(int meetingId)
        {
            var list = await _context.MinutesOfMeetings
                .Where(m => m.MeetingId == meetingId)
                .Select(m => new MinutesOfMeetingDto(m))
                .ToListAsync();

            if (list == null || list.Count == 0)
                return NotFound(new { message = $"No minutes of meeting found for Meeting ID {meetingId}." });

            return Ok(list);
        }
    }
}