using System;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class LocationDto
    {
        public long Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime LocationDateTime { get; set; }
    }
}
