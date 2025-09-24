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
    public class RoomFeatureController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public RoomFeatureController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetRoomFeature")]
        [Authorize]
        public async Task<IEnumerable<RoomFeatureDto>> Get()
        {
            return await _context.RoomFeatures.Select(rf => new RoomFeatureDto(rf)).ToListAsync();
        }

        [HttpGet("count", Name = "GetNbrRoomFeature")]
        public async Task<int> GetNbr()
        {
            return await _context.RoomFeatures.CountAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IEnumerable<RoomFeatureDto>> GetById(int id)
        {
            return await _context.RoomFeatures.Where(rf => rf.Id == id).Select(rf => new RoomFeatureDto(rf)).ToListAsync();
        }

        [HttpPost(Name = "setRoomFeature")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<RoomFeatureDto>> Set(NewRoomFeatureDto roomFeatureDto)
        {
            RoomFeature roomFeature = new RoomFeature
            {
                FeatureId = roomFeatureDto.FeatureId,
                RoomId = roomFeatureDto.RoomId
            };
            _context.RoomFeatures.Add(roomFeature);
            await _context.SaveChangesAsync();
            return _context.RoomFeatures.Where(rf => rf.Id == roomFeature.Id).Select(rf => new RoomFeatureDto(rf)).ToList();
        }

        [HttpPut(Name = "UpdateRoomFeature")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<RoomFeatureDto>> Update(RoomFeatureDto roomFeatureDto)
        {
            var roomFeature = _context.RoomFeatures.FirstOrDefault(rf => rf.Id == roomFeatureDto.Id);
            if (roomFeature != null)
            {
                roomFeature.FeatureId = roomFeatureDto.FeatureId;
                roomFeature.RoomId = roomFeatureDto.RoomId;
                await _context.SaveChangesAsync();
            }
            return _context.RoomFeatures.Where(rf => rf.Id == roomFeatureDto.Id).Select(rf => new RoomFeatureDto(rf)).ToList();
        }

        [HttpDelete(Name = "DeleteRoomFeature")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var roomFeature = _context.RoomFeatures.FirstOrDefault(rf => rf.Id == id);
            if (roomFeature == null)
            {
                return NotFound(new { message = $"RoomFeature with ID {id} not found." });
            }

            _context.RoomFeatures.Remove(roomFeature);
            _context.SaveChanges();
            return Ok(new { message = $"RoomFeature with ID {id} has been deleted." });
        }

        [HttpPost("batch")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoomFeatures([FromBody] List<NewRoomFeatureDto> roomFeatures)
        {
            if (roomFeatures == null || !roomFeatures.Any())
                return BadRequest(new { message = "No room features provided." });

            var added = new List<RoomFeatureDto>();
            foreach (var dto in roomFeatures)
            {
                // Prevent duplicates
                bool exists = await _context.RoomFeatures.AnyAsync(rf => rf.RoomId == dto.RoomId && rf.FeatureId == dto.FeatureId);
                if (!exists)
                {
                    var roomFeature = new RoomFeature
                    {
                        RoomId = dto.RoomId,
                        FeatureId = dto.FeatureId
                    };
                    _context.RoomFeatures.Add(roomFeature);
                    await _context.SaveChangesAsync();
                    added.Add(new RoomFeatureDto(roomFeature));
                }
            }
            return Ok(added);
        }
    }
}