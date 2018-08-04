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

        [HttpPost("delete/locations/{gpsEntryId?}")]
        public async Task<IActionResult> ClearGpsEntries(long? gpsEntryId)
        {
            var gpsEntries = await _context.GpsEntries
                .Where(x => !gpsEntryId.HasValue || gpsEntryId.Value == x.Id)
                .ToListAsync();

            foreach (var gpsEntry in gpsEntries)
            {
                _context.Remove(gpsEntry);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("last/{cowId?}")]
        public async Task<IActionResult> GetLastLocations(long? cowId)
        {
            var cowLocations = await _context.Cows
                .Where(x => x.GpsEntries.Any() && (!cowId.HasValue || cowId.Value == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntry = x.GpsEntries.OrderByDescending(ge => ge.DateTime).First(),
                    TemperatureEntry = x.TemperatureEntries.OrderByDescending(ge => ge.DateTime).First()
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowDto
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
                                   + cowLocation.GpsEntry.LatitudeMinutesDecimals / 600000,
                        Longitude = cowLocation.GpsEntry.LongitudeDeg
                                   + cowLocation.GpsEntry.LongitudeMinutes / 60
                                   + cowLocation.GpsEntry.LatitudeMinutesDecimals / 600000
                    }
                },
                Temperatures = new List<TemperatureDto>
                {
                    new TemperatureDto
                    {
                        Id = cowLocation.TemperatureEntry.Id,
                        Temperature = cowLocation.TemperatureEntry.Temperature,
                        DateTime = cowLocation.TemperatureEntry.DateTime
                    }
                }
            });

            return new JsonResult(result);
        }

        [HttpGet("last/fake/{numberOfCows}/{cowId?}")]
        public IActionResult GetFakeLastLocations(int numberOfCows, long? cowId)
        {
            var ids = new List<int>();
            for (var i = 0; i < numberOfCows; i++)
            {
                ids.Add(i + 1);
            }

            var result = ids.Select(id => new CowDto
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
                },
                Temperatures = new List<TemperatureDto>
                {
                    new TemperatureDto
                    {
                        Id = id,
                        DateTime = DateTime.UtcNow.ToLocalTime(),
                        Temperature = GetRandomTemperature()
                    }
                }
            });

            if (cowId.HasValue)
            {
                result = result.Where(x => x.CowId == cowId.Value).ToList();
            }

            return new JsonResult(result);
        }


        [HttpGet("{cowId?}")]
        public async Task<IActionResult> GetLocations(long? cowId)
        {
            var cowLocations = await _context.Cows
                .Where(x => x.GpsEntries.Any() && (!cowId.HasValue || cowId.Value == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntries = x.GpsEntries.OrderByDescending(ge => ge.DateTime),
                    TemperatureEntries = x.TemperatureEntries.OrderByDescending(ge => ge.DateTime)
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                Locations = cowLocation.GpsEntries.Select(ge => new LocationDto
                {
                    Id = ge.Id,
                    LocationDateTime = ge.DateTime.ToLocalTime(),
                    Latitude = ge.LatitudeDeg
                            + ge.LatitudeMinutes / 60
                            + ge.LatitudeMinutesDecimals / 600000,
                    Longitude = ge.LongitudeDeg
                            + ge.LongitudeMinutes / 60
                            + ge.LongitudeMinutesDecimals / 600000
                }).ToList(),
                Temperatures = cowLocation.TemperatureEntries.Select(x => new TemperatureDto
                {
                    Id = x.Id,
                    Temperature = x.Temperature,
                    DateTime = x.DateTime
                }).ToList()
            });

            return new JsonResult(result);
        }


        [HttpGet("fake/{numberOfCows}/{numberOfEntries}/{cowId?}")]
        public IActionResult GetFakeLocations(int numberOfCows, int numberOfEntries, long? cowId)
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

            var result = cowIds.Select(id => new CowDto
            {
                CowId = id,
                CowName = "Roberte",
                Locations = entryIds.Select(e => new LocationDto
                {
                    Id = entryIdMultiplier * id + e,
                    LocationDateTime = DateTime.UtcNow.ToLocalTime(),
                    Latitude = GetRandomLatitude(),
                    Longitude = GetRandomLongitude()
                }).ToList(),
                Temperatures = entryIds.Select(e => new TemperatureDto
                {
                    Id = entryIdMultiplier * id + e,
                    DateTime = DateTime.UtcNow.ToLocalTime(),
                    Temperature = GetRandomTemperature()
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

            return GetRandom(minRange, maxRange);
        }

        private double GetRandomLongitude()
        {
            const long minRange = 35105;
            const long maxRange = 35130;

            return GetRandom(minRange, maxRange);
        }

        private float GetRandomTemperature()
        {
            const int minTemp = 250;
            const int maxTemp = 360;

            return (float)GetRandom(minTemp, maxTemp, 10, 1);
        }

        private double GetRandom(long minNumber, long maxNumber, long divider = 10000, int precision = 1000000)
        {
            divider = divider * precision;
            minNumber = minNumber * precision;
            maxNumber = maxNumber * precision;

            var result = (int)(_random.NextDouble() * (maxNumber - minNumber + 1)) + minNumber;

            return (double)result / divider;
        }
    }
}
