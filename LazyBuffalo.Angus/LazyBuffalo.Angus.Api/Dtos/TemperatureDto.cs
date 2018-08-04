using System;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class TemperatureDto
    {
        public long Id { get; set; }
        public float Temperature { get; set; }
        public DateTime DateTime { get; set; }
    }
}
