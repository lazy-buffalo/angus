using LazyBuffalo.Angus.Api.Data;
using LazyBuffalo.Angus.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    GpsEntry = x.GpsEntries.OrderByDescending(ge => ge.DateTime).Last()
                }).ToListAsync();

            var result = cowLocations.Select(cowLocation => new CowLocationDto
            {
                CowId = cowLocation.Id,
                CowName = cowLocation.Name,
                LocationDateTime = cowLocation.GpsEntry.DateTime,
                Latitude = cowLocation.GpsEntry.LatitudeDeg
                           + (double)cowLocation.GpsEntry.LatitudeMinutes / 60
                           + (double)cowLocation.GpsEntry.LatitudeSecondes / 3600,
                Longitude = cowLocation.GpsEntry.LongitudeDeg
                           + (double)cowLocation.GpsEntry.LongitudeMinutes / 60
                           + (double)cowLocation.GpsEntry.LongitudeSecondes / 3600
            });

            return new JsonResult(result);
        }
    }
}
