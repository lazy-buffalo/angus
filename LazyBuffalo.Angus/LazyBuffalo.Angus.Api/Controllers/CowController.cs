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

        [HttpGet("last/{date}/{cowId?}")]
        public async Task<IActionResult> GetLast(DateTime date, long? cowId)
        {
            var cows = await _context.Cows
                .Where(x => !cowId.HasValue || cowId.Value == x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntry = x.GpsEntries
                        .Where(ge => ge.DateTime <= date)
                        .OrderByDescending(ge => ge.DateTime)
                        .FirstOrDefault(),
                    TemperatureEntry = x.TemperatureEntries
                        .OrderByDescending(ge => ge.DateTime)
                        .FirstOrDefault()
                }).ToListAsync();

            var result = cows.Select(cow => new CowDto
            {
                CowId = cow.Id,
                CowName = cow.Name,
                Locations = cow.GpsEntry != null ? new List<LocationDto>
                {
                    new LocationDto
                    {
                        Id = cow.GpsEntry.Id,
                        CowId = cow.Id,
                        DateTime = cow.GpsEntry.DateTime.ToLocalTime(),
                        Latitude = cow.GpsEntry.LatitudeDeg
                                   + cow.GpsEntry.LatitudeMinutes / 60
                                   + cow.GpsEntry.LatitudeMinutesDecimals / 600000,
                        Longitude = cow.GpsEntry.LongitudeDeg
                                   + cow.GpsEntry.LongitudeMinutes / 60
                                   + cow.GpsEntry.LatitudeMinutesDecimals / 600000
                    }
                } : new List<LocationDto>(),
                Temperatures = cow.TemperatureEntry != null ? new List<TemperatureDto>
                {
                    new TemperatureDto
                    {
                        Id = cow.TemperatureEntry.Id,
                        Temperature = cow.TemperatureEntry.Temperature,
                        DateTime = cow.TemperatureEntry.DateTime
                    }
                } : new List<TemperatureDto>()
            });

            return new JsonResult(result);
        }

        [HttpGet("fake/last/{numberOfCows}/{date}/{cowId?}")]
        public IActionResult GetFakeLast(int numberOfCows, DateTime date, long? cowId)
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
                        CowId = id,
                        DateTime = DateTime.UtcNow.ToLocalTime(),
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
        public async Task<IActionResult> Get(long? cowId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var endDate = DateTime.Today;

            if (start.HasValue)
            {
                endDate = (end ?? start.Value).Date.AddDays(1);

                if (endDate < start.Value.Date)
                    return BadRequest("End date must be greater or equal than start date.");
            }

            var cows = await _context.Cows
                .Where(x => !cowId.HasValue || cowId.Value == x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GpsEntries = x.GpsEntries
                         .Where(ge => !start.HasValue || (ge.DateTime.Date >= start.Value.Date && ge.DateTime.Date < endDate))
                         .OrderByDescending(ge => ge.DateTime),
                    TemperatureEntries = x.TemperatureEntries
                         .Where(te => !start.HasValue || (te.DateTime.Date >= start.Value.Date && te.DateTime.Date < endDate))
                         .OrderByDescending(ge => ge.DateTime)
                }).ToListAsync();

            var result = cows.Select(cow => new CowDto
            {
                CowId = cow.Id,
                CowName = cow.Name,
                Locations = cow.GpsEntries.Select(ge => new LocationDto
                {
                    Id = ge.Id,
                    CowId = cow.Id,
                    DateTime = ge.DateTime.ToLocalTime(),
                    Latitude = ge.LatitudeDeg
                            + ge.LatitudeMinutes / 60
                            + ge.LatitudeMinutesDecimals / 600000,
                    Longitude = ge.LongitudeDeg
                            + ge.LongitudeMinutes / 60
                            + ge.LongitudeMinutesDecimals / 600000
                }).ToList(),
                Temperatures = cow.TemperatureEntries.Select(x => new TemperatureDto
                {
                    Id = x.Id,
                    CowId = cow.Id,
                    Temperature = x.Temperature,
                    DateTime = x.DateTime
                }).ToList()
            }).ToList();

            HasStrangeLocation(result);
            HasStrangeTemperature(result);

            return new JsonResult(result);
        }


        [HttpGet("fake/{numberOfCows}/{multiplier}/{cowId?}")]
        public async Task<IActionResult> GetFake(int numberOfCows, long multiplier, long? cowId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var date = (start ?? DateTime.UtcNow).ToLocalTime();

            var cowIds = new List<int>();
            for (var i = 0; i < numberOfCows - 1; i++)
            {
                cowIds.Add(i + 1);
            }

            var marguerite = await _context.Cows
                .Include(x => x.GpsEntries)
                .Include(x => x.TemperatureEntries)
                .FirstOrDefaultAsync(x => x.Id == 2);

            var gpsEntryIdMultiplier = (int)Math.Pow(10, marguerite.GpsEntries.Count);
            var tempEntryIdMultiplier = (int)Math.Pow(10, marguerite.TemperatureEntries.Count);

            var gpsEntryMultiplier = 1 + _random.NextDouble() / multiplier;
            var tempEntryMultiplier = 1 + _random.NextDouble() / 10;

            var result = cowIds.Select(id => new CowDto
            {
                CowId = id,
                CowName = "Roberte",
                Locations = marguerite.GpsEntries.Select(ge => new LocationDto
                {
                    Id = gpsEntryIdMultiplier * id + ge.Id,
                    CowId = id,
                    DateTime = date,
                    Latitude = (ge.LatitudeDeg
                               + ge.LatitudeMinutes / 60
                               + ge.LatitudeMinutesDecimals / 600000) * gpsEntryMultiplier,
                    Longitude = (ge.LongitudeDeg
                                + ge.LongitudeMinutes / 60
                                + ge.LongitudeMinutesDecimals / 600000) * gpsEntryMultiplier
                }).ToList(),
                Temperatures = marguerite.TemperatureEntries.Select(e => new TemperatureDto
                {
                    Id = tempEntryIdMultiplier * id + e.Id,
                    DateTime = date,
                    Temperature = (float)(e.Temperature * tempEntryMultiplier)
                }).ToList()
            }).ToList();


            var gpsEntryIds = new List<int>();
            for (var i = 0; i < marguerite.GpsEntries.Count; i++)
            {
                gpsEntryIds.Add(i + 1);
            }

            var tempEntryIds = new List<int>();
            for (var i = 0; i < marguerite.TemperatureEntries.Count; i++)
            {
                tempEntryIds.Add(i + 1);
            }

            result.Add(new CowDto
            {

                CowId = numberOfCows,
                CowName = "Zelda",
                Locations = gpsEntryIds.Select(e => new LocationDto
                {
                    Id = gpsEntryIdMultiplier * numberOfCows + e,
                    CowId = numberOfCows,
                    DateTime = date,
                    Latitude = GetRandom(506026, 506030),
                    Longitude = GetRandom(35085, 35093)
                }).ToList(),
                Temperatures = tempEntryIds.Select(e => new TemperatureDto
                {
                    Id = tempEntryIdMultiplier * numberOfCows + e,
                    CowId = numberOfCows,
                    DateTime = date,
                    Temperature = GetRandomSickTemperature()
                }).ToList()
            });

            if (cowId.HasValue)
            {
                result = result.Where(x => x.CowId == cowId.Value).ToList();
            }

            HasStrangeLocation(result);
            HasStrangeTemperature(result);

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
            const int minTemp = 375;
            const int maxTemp = 395;

            return (float)GetRandom(minTemp, maxTemp, 10, 1);
        }

        private float GetRandomSickTemperature()
        {
            const int minTemp = 400;
            const int maxTemp = 420;

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

        private static void HasStrangeTemperature(IReadOnlyCollection<CowDto> cows)
        {
            var dataByHour = cows.SelectMany(x => x.Temperatures)
                .GroupBy(x => new DateTime(x.DateTime.Year, x.DateTime.Month, x.DateTime.Day, x.DateTime.Hour, x.DateTime.Minute / 15, 00))
                .ToList();

            var allStrangeCowIds = new List<long>();
            foreach (var group in dataByHour)
            {
                var strangeCowIdsForSet = GetCowsWithStrangeTemperature(group.ToList());
                allStrangeCowIds.AddRange(strangeCowIdsForSet);
            }

            var strangeCowIds = allStrangeCowIds
                .GroupBy(x => x)
                .Where(x => x.Count() > dataByHour.Count * 0.25)
                .Select(x => x.Key);

            foreach (var strangeCow in cows.Where(x => strangeCowIds.Contains(x.CowId)))
            {
                strangeCow.HasStrangeTemperature = true;
            }
        }

        private static IEnumerable<long> GetCowsWithStrangeTemperature(IReadOnlyCollection<TemperatureDto> temperatures)
        {
            var allTemperatures = temperatures
                .Select(x => x.Temperature)
                .ToArray();

            if (allTemperatures.Length < 4)
                return new List<long>();

            var medianIndex = (int)((float)allTemperatures.Length / 2);

            double median;
            if (medianIndex % 2 == 0)
            {
                median = (allTemperatures[medianIndex] + allTemperatures[medianIndex + 1]) / 2;
            }
            else
            {
                median = allTemperatures[medianIndex];
            }

            var q1Index = (int)((float)medianIndex / 2) - 1;

            double q1;
            if (q1Index % 2 == 0)
            {
                q1 = (allTemperatures[q1Index] + allTemperatures[q1Index + 1]) / 2;
            }
            else
            {
                q1 = allTemperatures[q1Index];
            }

            var q3Index = q1Index + medianIndex;

            double q3;
            if (q3Index % 2 == 0)
            {
                q3 = (allTemperatures[q3Index] + allTemperatures[q3Index + 1]) / 2;
            }
            else
            {
                q3 = allTemperatures[q3Index];
            }

            var iqr = q3 - q1;

            var diff = iqr * 1.5;


            return temperatures
                .Where(x => x.Temperature - median > diff)
                .Select(x => x.CowId)
                .Distinct();
        }

        private static void HasStrangeLocation(IReadOnlyCollection<CowDto> cows)
        {
            var locationsByHour = cows.SelectMany(x => x.Locations)
                .GroupBy(x => new DateTime(x.DateTime.Year, x.DateTime.Month, x.DateTime.Day, x.DateTime.Hour, x.DateTime.Minute / 15, 00))
                .ToList();

            var allStrangeCowIds = new List<long>();
            foreach (var locationsGroup in locationsByHour)
            {
                var strangeCowIdsForSet = GetCowsWithStrangeLocation(locationsGroup.ToList());
                allStrangeCowIds.AddRange(strangeCowIdsForSet);
            }

            var strangeCowIds = allStrangeCowIds
                .GroupBy(x => x)
                .Where(x => x.Count() > locationsByHour.Count * 0.25)
                .Select(x => x.Key);

            foreach (var strangeCow in cows.Where(x => strangeCowIds.Contains(x.CowId)))
            {
                strangeCow.HasStrangeLocation = true;
            }
        }

        private static IEnumerable<long> GetCowsWithStrangeLocation(IReadOnlyCollection<LocationDto> locations)
        {
            if (locations.Count < 4)
                return new List<long>();

            var allLongitude = locations.Select(x => x.Longitude)
                .OrderBy(x => x)
                .ToArray();

            var allLatitude = locations.Select(x => x.Latitude)
                .OrderBy(x => x)
                .ToArray();

            var medianIndex = (int)((float)locations.Count / 2);

            double medianLongitude;
            double medianLatitude;
            if (medianIndex % 2 == 0)
            {
                medianLongitude = (allLongitude[medianIndex] + allLongitude[medianIndex + 1]) / 2;
                medianLatitude = (allLatitude[medianIndex] + allLatitude[medianIndex + 1]) / 2;
            }
            else
            {
                medianLongitude = allLongitude[medianIndex];
                medianLatitude = allLatitude[medianIndex];
            }

            var q1Index = (int)((float)medianIndex / 2) - 1;

            double q1Longitude;
            double q1Latitude;
            if (q1Index % 2 == 0)
            {
                q1Longitude = (allLongitude[q1Index] + allLongitude[q1Index + 1]) / 2;
                q1Latitude = (allLatitude[q1Index] + allLatitude[q1Index + 1]) / 2;
            }
            else
            {
                q1Longitude = allLongitude[q1Index];
                q1Latitude = allLatitude[q1Index];
            }

            var q3Index = q1Index + medianIndex;

            double q3Longitude;
            double q3Latitude;
            if (q3Index % 2 == 0)
            {
                q3Longitude = (allLongitude[q3Index] + allLongitude[q3Index + 1]) / 2;
                q3Latitude = (allLatitude[q3Index] + allLatitude[q3Index + 1]) / 2;
            }
            else
            {
                q3Longitude = allLongitude[q3Index];
                q3Latitude = allLatitude[q3Index];
            }

            var iqrLongitude = q3Longitude - q1Longitude;
            var iqrLatitude = q3Latitude - q1Latitude;

            var diffLongitude = iqrLongitude * 1.5;
            var diffLatitude = iqrLatitude * 1.5;

            return locations
                .Where(x =>
                    (x.Longitude - medianLongitude) > diffLongitude || (x.Latitude - medianLatitude) > diffLatitude)
                .Select(x => x.CowId)
                .Distinct();
        }
    }
}
