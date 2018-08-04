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

            var result = cowLocations.Select(cowLocation => new CowLocationDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                Locations = new List<LocationDto>
                {
                    new LocationDto
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
                }
            });

            return new JsonResult(result);
        }

        [HttpGet("locations/last/fake/{numberOfCows}/{cowId?}")]
        public IActionResult GetFakeLastLocations(int numberOfCows, int? cowId)
        {
            var ids = new List<int>();
            for (var i = 0; i < numberOfCows; i++)
            {
                ids.Add(i + 1);
            }

            var result = ids.Select(id => new CowLocationDto
            {
                CowId = id,
                CowName = "Roberte",
                Locations = new List<LocationDto>
                {
                    new LocationDto
                    {
                        Id = id,
                        LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                        Latitude = GetRandomLatitude(),
                        Longitude = GetRandomLongitude()
                    }
                }
            });

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


        [HttpGet("locations/fake/{numberOfCows}/{numberOfEntries}/{cowId?}")]
        public IActionResult GetFakeLocations(int numberOfCows, int numberOfEntries, int? cowId)
        {
            var cowIds = new List<int>();
            for (var i = 0; i < numberOfCows; i++)
            {
                cowIds.Add(i + 1);
            }

            var entryIds = new List<int>();
            for (var i = 0; i < numberOfEntries; i++)
            {
                entryIds.Add(i + 1);
            }

            var entryIdMultiplier = (int)Math.Pow(10, numberOfEntries.ToString().Length);

            var result = cowIds.Select(id => new CowLocationDto
            {
                CowId = id,
                CowName = "Roberte",
                Locations = entryIds.Select(e => new LocationDto
                {
                    Id = entryIdMultiplier * id + e,
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = GetRandomLatitude(),
                    Longitude = GetRandomLongitude()
                }).ToList()
            });

            if (cowId.HasValue)
            {
                result = result.Where(x => x.CowId == cowId.Value).ToList();
            }

            return new JsonResult(result);
        }


        private static double GetRandomLatitude()
        {
            const int minRange = 35105;
            const int maxRange = 35130;

            return GetRandomCoordinate(minRange, maxRange, 1000);
        }

        private static double GetRandomLongitude()
        {
            const int minRange = 506012;
            const int maxRange = 506020;

            return GetRandomCoordinate(minRange, maxRange, 10000);
        }

        private static double GetRandomCoordinate(double minNumber, double maxNumber, int divider)
        {
            var result = new Random(DateTime.Now.Millisecond).NextDouble() * (minNumber - maxNumber) + minNumber;

            return result / divider;
        }
    }
}
