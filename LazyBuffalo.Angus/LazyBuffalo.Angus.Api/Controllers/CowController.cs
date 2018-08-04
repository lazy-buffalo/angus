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
        private readonly Random _random;

        public CowController(AngusDbContext context)
        {
            _context = context;
            _random = new Random(DateTime.Now.Millisecond);
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


        private double GetRandomLatitude()
        {
            const long minRange = 506012;
            const long maxRange = 506020;

            return GetRandomCoordinate(minRange, maxRange);
        }

        private double GetRandomLongitude()
        {
            const long minRange = 35105;
            const long maxRange = 35130;

            return GetRandomCoordinate(minRange, maxRange);
        }

        private double GetRandomCoordinate(long minNumber, long maxNumber, long divider = 10000)
        {
            const int precision = 1000000000;
            divider = divider * precision;
            minNumber = minNumber * precision;
            maxNumber = maxNumber * precision;

            var result = (int)(_random.NextDouble() * (maxNumber - minNumber + 1)) + minNumber;

            return (double)result / divider;
        }
    }
}
