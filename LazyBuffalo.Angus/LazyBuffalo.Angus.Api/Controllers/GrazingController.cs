using LazyBuffalo.Angus.Api.Data;
using LazyBuffalo.Angus.Api.Dtos;
using LazyBuffalo.Angus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LazyBuffalo.Angus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrazingController : ControllerBase
    {
        private readonly AngusDbContext _context;

        public GrazingController(AngusDbContext context)
        {
            _context = context;
        }

        // GET: api/Grazing
        [HttpGet]
        public IEnumerable<GrazingDto> Get()
        {
            return _context.Grazings
                .Select(x => new GrazingDto
                {
                    Id = x.Id,
                    Coordinates = x.Coordinates
                    .Select(c => new GoogleCoordinateDto
                    {
                        Longitude = c.Longitude,
                        Latitude = c.Latitude
                    }).ToList()
                });
        }

        // GET: api/Grazing/5
        [HttpGet("{id}", Name = "Get")]
        public GrazingDto Get(long id)
        {
            return _context.Grazings
                .Select(x => new GrazingDto
                {
                    Id = x.Id,
                    Coordinates = x.Coordinates
                        .Select(c => new GoogleCoordinateDto
                        {
                            Longitude = c.Longitude,
                            Latitude = c.Latitude
                        }).ToList()
                })
                .FirstOrDefault(x => x.Id == id);
        }

        // POST: api/Grazing
        [HttpPost]
        public async Task Post([FromBody] GrazingDto value)
        {
            var grazing = new Grazing
            {
                Coordinates = value.Coordinates
                    .Select(x => new Coordinate
                    {
                        Longitude = x.Longitude,
                        Latitude = x.Latitude
                    }).ToList()
            };

            await _context.Grazings.AddAsync(grazing);
            await _context.SaveChangesAsync();
        }

        // PUT: api/Grazing/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] GrazingDto value)
        {
            var grazing = _context.Grazings.FirstOrDefault(x => x.Id == id);
            if (grazing == null) return NotFound();

            grazing.Coordinates = value.Coordinates
                .Select(x => new Coordinate
                {
                    Longitude = x.Longitude,
                    Latitude = x.Latitude
                }).ToList();

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Grazing/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var grazing = _context.Grazings.FirstOrDefault(x => x.Id == id);
            if (grazing == null) return NotFound();

            _context.Grazings.Remove(grazing);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
