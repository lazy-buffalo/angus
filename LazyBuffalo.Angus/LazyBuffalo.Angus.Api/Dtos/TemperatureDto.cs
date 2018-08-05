using System;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class TemperatureDto : ICowDataEntry
    {
        public long Id { get; set; }
        public float Temperature { get; set; }
        public DateTime DateTime { get; set; }
        public long CowId { get; set; }
    }
}
