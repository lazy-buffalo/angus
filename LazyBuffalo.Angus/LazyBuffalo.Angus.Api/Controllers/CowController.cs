using LazyBuffalo.Angus.Api.Data;
using LazyBuffalo.Angus.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
                    GpsEntry = x.GpsEntries.Last()
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowLocationDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                LocationDateTime = cowLocation.GpsEntry.DateTime,
                Latidude = Convert.ToDouble(cowLocation.GpsEntry.LatitudeDeg + "." +
                                            cowLocation.GpsEntry.LatitudeMinutes +
                                            cowLocation.GpsEntry.LatitudeSecondes),
                Longitude = Convert.ToDouble(cowLocation.GpsEntry.LongitudeDeg + "." +
                                             cowLocation.GpsEntry.LongitudeMinutes +
                                             cowLocation.GpsEntry.LongitudeSecondes)
            });

            return new JsonResult(result);
        }
    }
}
