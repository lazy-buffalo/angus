using System;

namespace LazyBuffalo.Angus.Api.Models
{
    public class TemperatureEntry : IEntry
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public float Temperature { get; set; }
        public long CowId { get; set; }
        public Cow Cow { get; set; }
    }
}
