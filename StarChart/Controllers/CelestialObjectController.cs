using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.SingleOrDefault(c => c.Id == id);
            if (celestialObject == null)
                return NotFound();

            var satellites = _context.CelestialObjects
                .Where(s => s.OrbitedObjectId == celestialObject.Id)
                .ToList();

            celestialObject.Satellites = satellites;
            _context.SaveChanges();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name);
            if (!celestialObjects.Any())
                return NotFound();

            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(celestialObjects);
        }
    }
}
