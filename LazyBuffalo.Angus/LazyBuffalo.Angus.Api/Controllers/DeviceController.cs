using LazyBuffalo.Angus.Api.Data;
using LazyBuffalo.Angus.Api.Dtos;
using LazyBuffalo.Angus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LazyBuffalo.Angus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly AngusDbContext _context;

        public DeviceController(AngusDbContext context)
        {
            _context = context;
        }

        // POST api/device
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Rootobject rootobject)
        {
            var playload = Convert.FromBase64String(rootobject.PayloadRaw);

            var cow = await _context.Cows.FirstOrDefaultAsync(x => x.HardwareSerial == rootobject.HardwareSerial);
            if (cow == null)
            {
                cow = new Cow
                {
                    Name = "Marguerite",
                    HardwareSerial = rootobject.HardwareSerial
                };

                await _context.Cows.AddAsync(cow);
            }

            var gpsEntry = new GpsEntry
            {
                Cow = cow,
                DateTime = rootobject.Metadata.Time,
                LatitudeDeg = Convert.ToInt32(playload[0]),
                LatitudeMinutes = Convert.ToInt32(playload[1]),
                LatitudeSecondes = Convert.ToInt32(Convert.ToInt32(playload[2]) + Convert.ToInt32(playload[3]).ToString()),
                LatitudeDirection = Convert.ToChar(playload[4]),
                LongitudeDeg = Convert.ToInt32(playload[5]),
                LongitudeMinutes = Convert.ToInt32(playload[6]),
                LongitudeSecondes = Convert.ToInt32(Convert.ToInt32(playload[7]) + Convert.ToInt32(playload[8]).ToString()),
                LongitudeDirection = Convert.ToChar(playload[9])
            };

            cow.GpsEntries.Add(gpsEntry);

            var temperatureEntry = new TemperatureEntry
            {
                Cow = cow,
                DateTime = rootobject.Metadata.Time,
                Temperature = Convert.ToSingle(Convert.ToInt32(playload[10]) + "." + Convert.ToInt32(playload[11]))
            };

            cow.TemperatureEntries.Add(temperatureEntry);

            var positionEntry = new PositionEntry
            {
                Cow = cow,
                DateTime = rootobject.Metadata.Time,
                IsUp = Convert.ToBoolean(Convert.ToInt32(playload[12])),
                X = Convert.ToInt32(playload[13]),
                Y = Convert.ToInt32(playload[14]),
                Z = Convert.ToInt32(playload[15])
            };

            cow.PositionEntries.Add(positionEntry);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
