using LazyBuffalo.Angus.Api.Data;
using LazyBuffalo.Angus.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LazyBuffalo.Angus.Api.Controllers
{
    [Route("api/cows")]
    [ApiController]
    public class CowController : ControllerBase
    {
        private readonly AngusDbContext _context;

        public CowController(AngusDbContext context)
        {
            _context = context;
        }

        // GET api/cows/locations
        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations()
        {
            var cowLocations = await _context.Cows
                .Where(x => x.GpsEntries.Any())
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntry = x.GpsEntries.OrderByDescending(ge => ge.DateTime).First()
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowLocationDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                LocationDateTime = cowLocation.GpsEntry.DateTime.ToLocalTime(),
                Latitude = cowLocation.GpsEntry.LatitudeDeg
                           + cowLocation.GpsEntry.LatitudeMinutes / 60
                           + cowLocation.GpsEntry.LatitudeSecondes / 3600,
                Longitude = cowLocation.GpsEntry.LongitudeDeg
                           + cowLocation.GpsEntry.LongitudeMinutes / 60
                           + cowLocation.GpsEntry.LongitudeSecondes / 3600
            });

            return new JsonResult(result);
        }

        // GET api/cows/locations/fake
        [HttpGet("locations/fake")]
        public IActionResult GetFakeLocations()
        {
            var result = new List<CowLocationDto>
            {
                new CowLocationDto
                {
                    CowId = 1,
                    CowName = "Roberte",
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = 50.60192548443745,
                    Longitude = 3.51157760612557
                },
                new CowLocationDto
                {
                    CowId = 2,
                    CowName = "Marguerite",
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = 50.601907041658286,
                    Longitude = 3.5116938560830704
                },
                new CowLocationDto
                {
                    CowId = 3,
                    CowName = "Kiri",
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = 50.60185369942146,
                    Longitude = 3.511409115792844
                },
                new CowLocationDto
                {
                    CowId = 4,
                    CowName = "Amandine",
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = 50.60186164401623,
                    Longitude = 3.511526706854852
                },
                new CowLocationDto
                {
                    CowId = 5,
                    CowName = "Marcele",
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = 50.60189342237546,
                    Longitude = 3.511735493023366
                },
                new CowLocationDto
                {
                    CowId = 6,
                    CowName = "Zelda",
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = 50.601936558444415,
                    Longitude = 3.51033373468681
                }
            };

            return new JsonResult(result);
        }
    }
}
