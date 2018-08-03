using System.Collections.Generic;

namespace LazyBuffalo.Angus.Api.Models
{
    public class Cow
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string HardwareSerial { get; set; }

        public List<GpsEntry> GpsEntries { get; set; }
        public List<TemperatureEntry> TemperatureEntries { get; set; }

        public Cow()
        {
            GpsEntries = new List<GpsEntry>();
            TemperatureEntries = new List<TemperatureEntry>();
        }
    }
}
