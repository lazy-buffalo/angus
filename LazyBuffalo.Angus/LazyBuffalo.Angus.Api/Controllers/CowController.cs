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

        [HttpGet("locations/last/{cowId?}")]
        public async Task<IActionResult> GetLastLocations(int? cowId)
        {
            var cowLocations = await _context.Cows
                .Where(x => x.GpsEntries.Any() && (!cowId.HasValue || cowId.Value == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntry = x.GpsEntries.OrderByDescending(ge => ge.DateTime).First()
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowLastLocationDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                Location = new LocationDto
                {
                    Id = cowLocation.GpsEntry.Id,
                    LocationDateTime = cowLocation.GpsEntry.DateTime.ToLocalTime(),
                    Latitude = cowLocation.GpsEntry.LatitudeDeg
                               + cowLocation.GpsEntry.LatitudeMinutes / 60
                               + cowLocation.GpsEntry.LatitudeSecondes / 3600,
                    Longitude = cowLocation.GpsEntry.LongitudeDeg
                               + cowLocation.GpsEntry.LongitudeMinutes / 60
                               + cowLocation.GpsEntry.LongitudeSecondes / 3600
                }
            });

            return new JsonResult(result);
        }

        [HttpGet("locations/last/fake/{cowId?}")]
        public IActionResult GetFakeLastLocations(int? cowId)
        {
            var result = new List<CowLastLocationDto>
            {
                new CowLastLocationDto
                {
                    CowId = 1,
                    CowName = "Roberte",
                    Location = new LocationDto
                    {
                        Id = 1,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = 50.60192548443745,
                        Longitude = 3.51157760612557
                    }
                },
                new CowLastLocationDto
                {
                    CowId = 2,
                    CowName = "Marguerite",
                    Location = new LocationDto
                    {
                        Id = 2,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = 50.601907041658286,
                        Longitude = 3.5116938560830704
                    }
                },
                new CowLastLocationDto
                {
                    CowId = 3,
                    CowName = "Kiri",
                    Location = new LocationDto
                    {
                        Id = 3,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = 50.60185369942146,
                        Longitude = 3.511409115792844
                    }
                },
                new CowLastLocationDto
                {
                    CowId = 4,
                    CowName = "Amandine",
                    Location = new LocationDto
                    {
                        Id = 4,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = 50.60186164401623,
                        Longitude = 3.511526706854852
                    }
                },
                new CowLastLocationDto
                {
                    CowId = 5,
                    CowName = "Marcele",
                    Location = new LocationDto
                    {
                        Id = 5,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = 50.60189342237546,
                        Longitude = 3.511735493023366
                    }
                },
                new CowLastLocationDto
                {
                    CowId = 6,
                    CowName = "Zelda",
                    Location = new LocationDto
                    {
                        Id = 6,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = 50.601936558444415,
                        Longitude = 3.51033373468681
                    }
                }
            };

            if (cowId.HasValue)
            {
                result = result.Where(x => x.CowId == cowId.Value).ToList();
            }

            return new JsonResult(result);
        }


        [HttpGet("locations/{cowId?}")]
        public async Task<IActionResult> GetLocations(int? cowId)
        {
            var cowLocations = await _context.Cows
                .Where(x => x.GpsEntries.Any() && (!cowId.HasValue || cowId.Value == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntry = x.GpsEntries.OrderByDescending(ge => ge.DateTime)
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowLocationDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                Locations = cowLocation.GpsEntry.Select(ge => new LocationDto
                {
                    Id = ge.Id,
                    LocationDateTime = ge.DateTime.ToLocalTime(),
                    Latitude = ge.LatitudeDeg
                           + ge.LatitudeMinutes / 60
                           + ge.LatitudeSecondes / 3600,
                    Longitude = ge.LongitudeDeg
                            + ge.LongitudeMinutes / 60
                            + ge.LongitudeSecondes / 3600
                }).ToList()
            });

            return new JsonResult(result);
        }
    }
}
