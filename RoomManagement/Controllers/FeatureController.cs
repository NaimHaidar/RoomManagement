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
    public class FeatureController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public FeatureController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetFeature")]
        [Authorize]
        public async Task<IEnumerable<FeatureDto>> Get()
        {
            return await _context.Features.Select(f => new FeatureDto(f)).ToListAsync();
        }

        [HttpGet("count", Name = "GetNbrFeature")]
        public async Task<int> GetNbr()
        {
            return await _context.Features.CountAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IEnumerable<FeatureDto>> GetById(int id)
        {
            return await _context.Features.Where(f => f.Id == id).Select(f => new FeatureDto(f)).ToListAsync();
        }

        [HttpPost(Name = "setFeature")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<FeatureDto>> Set(NewFeatureDto featureDto)
        {
            Feature feature = new Feature(featureDto.Feature1 ?? string.Empty);
            _context.Features.Add(feature);
            await _context.SaveChangesAsync();
            return _context.Features.Where(f => f.Id == feature.Id).Select(f => new FeatureDto(f)).ToList();
        }

        [HttpPut(Name = "UpdateFeature")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<FeatureDto>> Update(FeatureDto featureDto)
        {
            var feature = _context.Features.FirstOrDefault(f => f.Id == featureDto.Id);
            if (feature != null)
            {
                feature.Feature1 = featureDto.Feature1;
                await _context.SaveChangesAsync();
            }
            return _context.Features.Where(f => f.Id == featureDto.Id).Select(f => new FeatureDto(f)).ToList();
        }

        [HttpDelete(Name = "DeleteFeature")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var feature = _context.Features.FirstOrDefault(f => f.Id == id);
            if (feature == null)
            {
                return NotFound(new { message = $"Feature with ID {id} not found." });
            }

            _context.Features.Remove(feature);
            _context.SaveChanges();
            return Ok(new { message = $"Feature with ID {id} has been deleted." });
        }
    }
}
