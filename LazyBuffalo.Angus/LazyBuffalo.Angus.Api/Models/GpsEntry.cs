using System;

namespace LazyBuffalo.Angus.Api.Models
{
    public class GpsEntry : IEntry
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }

        public int LatitudeDeg { get; set; }
        public int LatitudeMinutes { get; set; }
        public int LatitudeSecondes { get; set; }
        public char LatitudeDirection { get; set; }

        public int LongitudeDeg { get; set; }
        public int LongitudeMinutes { get; set; }
        public int LongitudeSecondes { get; set; }
        public char LongitudeDirection { get; set; }

        public long CowId { get; set; }
        public Cow Cow { get; set; }
    }
}
