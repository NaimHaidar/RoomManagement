using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;
using Microsoft.AspNetCore.Authorization; // Added for Authorization

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // Apply authorization to the entire controller
    public class FeatureController : ControllerBase
    {
        private readonly RoomManagementDBContext _context;

        public FeatureController(RoomManagementDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetFeature")]
        [AllowAnonymous]
        public IEnumerable<Feature> Get()
        {
            return _context.Features.ToList();
        }

        [HttpGet("{id}")]
        [AllowAnonymous] 
        public IEnumerable<Feature> GetById(int id)
        {
            return _context.Features.Where(f => f.Id == id).ToList();
        }

        [HttpPost(Name = "setFeature")]
        [Authorize(Roles = "Admin")] 
        public IEnumerable<Feature> Set(string name)
        {
            Feature feature = new Feature(name);
            _context.Features.Add(feature);
            _context.SaveChanges();
            return _context.Features.Where(f => f.Id == feature.Id).ToList();
        }

        [HttpPut(Name = "UpdateFeature")]
        [Authorize(Roles = "Admin")] 
        public IEnumerable<Feature> Update(int id, string name)
        {
            var feature = _context.Features.Where(f => f.Id == id).FirstOrDefault();
            if (feature != null)
            {
                feature.Feature1 = name;
                _context.SaveChanges();
            }
            return _context.Features.Where(f => f.Id == id).ToList();
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
