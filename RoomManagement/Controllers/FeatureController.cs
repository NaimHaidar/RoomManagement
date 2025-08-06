using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository;
using RoomManagement.Repository.Models;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeatureController : ControllerBase
    {
    

        [HttpGet(Name = "GetFeature")]
        public IEnumerable<Feature> Get()
        { using (var context = new RoomManagementDBContext())
            {
                return context.Features.ToList();
            }
        }

        [HttpGet("{id}")]
        public IEnumerable<Feature> GetById(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
               
                return context.Features.Where(f => f.Id == id).ToList();
            }
        }

        [HttpPost(Name ="setFeature")]
        public IEnumerable<Feature> set(string name)
        {
            using (var context = new RoomManagementDBContext())
            {
                Feature feature = new Feature(name);
                context.Features.Add(feature);
                context.SaveChanges();
                return context.Features.Where(f => f.Id == feature.Id).ToList();
            }
        }
        [HttpPut(Name = "UpdateFeature")]
        public IEnumerable<Feature> Update(int id ,string name)
        {
            using (var context = new RoomManagementDBContext())
            {
                var feature = context.Features.Where(f => f.Id == id).FirstOrDefault();
                if (feature != null)
                {
                    feature.Feature1 = name;
                    context.SaveChanges(); 
                }
               
               
                return context.Features.Where(f => f.Id == id).ToList();
            }
        }
        [HttpDelete( Name = "DeleteFeature")]
        public IActionResult Delete(int id)
        {
            using (var context = new RoomManagementDBContext())
            {
                var feature = context.Features.FirstOrDefault(f => f.Id == id);
                if (feature == null)
                {
                    return NotFound(new { message = $"Feature with ID {id} not found." });
                }

                context.Features.Remove(feature);
                context.SaveChanges();
                return Ok(new { message = $"Feature with ID {id} has been deleted." });
            }
        }


    }
}
