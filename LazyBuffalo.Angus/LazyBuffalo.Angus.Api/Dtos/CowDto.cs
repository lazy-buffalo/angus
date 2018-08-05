using System.Collections.Generic;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class CowDto
    {
        public long CowId { get; set; }
        public string CowName { get; set; }
        public bool HasStrangeLocation { get; set; }
        public bool HasStrangeTemperature { get; set; }

        public List<LocationDto> Locations { get; set; }
        public List<TemperatureDto> Temperatures { get; set; }
    }
}

