using System;

namespace LazyBuffalo.Angus.Api.Models
{
    public class GpsEntry : IEntry
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }

        public double LatitudeDeg { get; set; }
        public double LatitudeMinutes { get; set; }
        public double LatitudeMinutesDecimals { get; set; }
        public double LatitudeSecondes { get; set; }
        public char LatitudeDirection { get; set; }

        public double LongitudeDeg { get; set; }
        public double LongitudeMinutes { get; set; }
        public double LongitudeSecondes { get; set; }
        public double LongitudeMinutesDecimals { get; set; }
        public char LongitudeDirection { get; set; }

        public long CowId { get; set; }
        public Cow Cow { get; set; }
    }
}
